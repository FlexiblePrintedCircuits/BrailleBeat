using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public float DotPadding { get => .35f; }

	[SerializeField] string FilePath;
	[SerializeField] string ClipPath;

	[SerializeField] Button Play;

	[SerializeField] GameObject Tenji;
	[SerializeField] Sprite frameSprite;

	[SerializeField] Transform Center;
	[SerializeField] Transform BeatPoint;
	[SerializeField] Transform ButtonParent;
	[SerializeField] GameObject CharacterPrefab;
	[SerializeField] Transform CharacterSpawnPoint;

	[SerializeField] Text scoreText;
	[SerializeField] Text comboText;

	[SerializeField] ScoreController scoreController;

	string Title;
	int BPM;
	AudioSource music;
	Dictionary<int, List<GameObject>> Notes;

	float PlayTime;
	float Distance;
	float During;
	bool isPlaying;
	int noteIndex;

	float CheckRange = 120;
	float BeatRange = 80;
	List<float> NoteTimings;
	List<GameObject> CharacterNotes;

	void OnEnable()
	{
		// setup
		Distance = Math.Abs(BeatPoint.position.x - Center.position.x);
		During = 3 * 1000;
		isPlaying = false;
		noteIndex = 0;
		scoreController.onChanged += ScoreController_onChanged;

		Play.onClick
		  .AsObservable()
		  .Subscribe(_ =>
		  {
			  loadChart();
			  play();
		  });

		this.UpdateAsObservable()
			.Where(_ => isPlaying)
			.Where(_ => !music.isPlaying)
			.Subscribe(_ => end());

		// ノーツを発射するタイミングかチェックし、go関数を発火
		this.UpdateAsObservable()
			.Where(_ => isPlaying)
			.Where(_ => NoteTimings.Count > noteIndex)
			.Where(_ => NoteTimings[noteIndex] <= ((Time.time * 1000 - PlayTime) + During))
			.Subscribe(_ =>
			{
				for (int i = 0; i < 9; i++)
				{
					Notes[i][noteIndex].GetComponent<DotController>().go(Distance, During);
				}
				CharacterNotes[noteIndex].SetActive(true);
				CharacterNotes[noteIndex].GetComponent<DotController>().go(Distance, During);
				noteIndex++;
			});

		music = GetComponent<AudioSource>();
	}

	private void ScoreController_onChanged(object sender, EventArgs e)
	{
		scoreText.text = scoreController.Score.ToString();
		comboText.text = scoreController.Combo.ToString();
	}

	void loadChart()
	{
		Notes = new Dictionary<int, List<GameObject>>();
		NoteTimings = new List<float>();
		CharacterNotes = new List<GameObject>();
		for (int i = 0; i < 9; i++)
		{
			Notes[i] = new List<GameObject>();

			// ボタン
			var button = ButtonParent.Find($"Button{i}");

			button.GetComponent<ButtonController>().OnButtonDown += GameManager_OnButtonPressed;
		}

		// ノーツjson読み込み
		string jsonText = Resources.Load<TextAsset>(FilePath).ToString();

		JsonNode json = JsonNode.Parse(jsonText);
		Title = json["title"].Get<string>();
		BPM = int.Parse(json["bpm"].Get<string>());

		foreach (var tenji in json["notes"])
		{
			var type = tenji["type"];
			int count = type.Count;

			var timingMs = float.Parse(tenji["timing"].Get<string>());
			NoteTimings.Add(timingMs);
			// ノーツの下に表示するひらがなを生成
			string character = tenji["character"].Get<string>();
			GameObject characterNote = Instantiate(CharacterPrefab, CharacterSpawnPoint.position, Quaternion.identity);
			characterNote.transform.Find("Canvas").Find("CharText").GetComponent<Text>().text = character;
			characterNote.GetComponent<DotController>().Timing = timingMs;
			characterNote.GetComponent<DotController>().IsFrame = false;
			characterNote.SetActive(false);
			CharacterNotes.Add(characterNote);
			for (var y = 0; y < count; y++)
			{
				var rows = type[y];
				for (var x = 0; x < rows.Count; x++)
				{
					// ノーツ生成
					var note = int.Parse(rows[x].Get<string>());
					var index = x + y * rows.Count;

					var padding = new Vector3(x - 1, y - 1) * DotPadding;

					GameObject Dot;
					Dot = Instantiate(Tenji, Center.position + padding, Quaternion.identity);
					var isFrame = (note == 0);
					if (isFrame)
					{
						Dot.GetComponent<SpriteRenderer>().sprite = frameSprite;
					}

					var controller = Dot.GetComponent<DotController>();
					controller.Timing = timingMs;
					controller.IsFrame = isFrame;
					Notes[index].Add(Dot);
				}
			}

		}

		// 曲読み込み
		music.clip = Resources.Load<AudioClip>(ClipPath);
	}

	private void GameManager_OnButtonPressed(object sender, int index)
	{
		beat(index, Time.time * 1000 - PlayTime);
	}

	void play()
	{
		music.Stop();
		music.volume = .3f;
		music.Play();

		PlayTime = Time.time * 1000;
		isPlaying = true;
		Debug.Log("Game Start!");
	}

	void beat(int index, float timing)
	{
		float minDiff = -1;
		int minDiffIndex = -1;

		for (int i = 0; i < NoteTimings.Count; i++)
		{
			if (NoteTimings[i] > 0)
			{
				float diff = Math.Abs(NoteTimings[i] - timing);
				if (minDiff == -1 || minDiff > diff)
				{
					minDiff = diff;
					minDiffIndex = i;
				}
			}
		}

		if (minDiff != -1 & minDiff < CheckRange)
		{
			var touchedNote = Notes[index][minDiffIndex];

			if (touchedNote.GetComponent<DotController>().IsFrame)
			{
				return;
			}

			if (minDiff < BeatRange)
			{
				touchedNote.SetActive(false);
				scoreController.Success();
				scoreText.text = scoreController.Score.ToString();

				Debug.Log(" success.");
			}
			else
			{
				touchedNote.SetActive(false);
				scoreController.Failure();

				Debug.Log(" failure.");
			}

			// 空ドットを一緒に非表示にする
			for (int i = 0; i < 9; i++)
			{
				var note = Notes[i][minDiffIndex];
				var controller = note.GetComponent<DotController>();
				if (controller.IsFrame)
				{
					note.SetActive(false);
				}
			}
			// ひらがな の部分も非表示にする
			CharacterNotes[index].SetActive(false);
		}
		else
		{
			Debug.Log("through");
		}
	}

	void end()
	{
		scoreController.UpdateScore();

		var store = ScoreData.Instance;
		store.maxCombo = scoreController.MaxCombo;
		store.goodCount = scoreController.Good;
		store.missCount = scoreController.Miss;
		store.score = scoreController.Score;

		SceneManager.LoadScene("ResultScene");
	}
}