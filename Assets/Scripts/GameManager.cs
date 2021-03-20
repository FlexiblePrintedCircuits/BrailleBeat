using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

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
	List<string> Characters;

	void OnEnable()
	{
		// setup
		Distance = Math.Abs(BeatPoint.position.x - Center.position.x);
		During = 3 * 1000;
		isPlaying = false;
		noteIndex = 0;

		Play.onClick
		  .AsObservable()
		  .Subscribe(_ =>
		  {
			  loadChart();
			  play();
		  });


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

				noteIndex++;
			});

		music = GetComponent<AudioSource>();
	}

	void loadChart()
	{
		Notes = new Dictionary<int, List<GameObject>>();
		NoteTimings = new List<float>();
		for (int i = 0; i < 9; i++)
		{
			Notes[i] = new List<GameObject>();

			// ボタン
			var button = ButtonParent.Find($"Button{i}");

			button.GetComponent<ObservableEventTrigger>()
				.OnPointerDownAsObservable()
				.Subscribe((_) =>
				{
					var index = button.GetComponent<ButtonState>().Index;
					beat(index, Time.time * 1000 - PlayTime);
				});
		}

		// キーボード判定
		var keys = new KeyCode[] { KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Q, KeyCode.W, KeyCode.E };
		for (int i = 0; i < keys.Length; i++)
		{
			var key = keys[i];
			var index = i;
			this.UpdateAsObservable()
				.Where(_ => Input.GetKeyDown(key))
				.Subscribe(_ =>
				{
					beat(index, Time.time * 1000 - PlayTime);
				});
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
			Characters.Add(tenji["character"].Get<string>());
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
			if (minDiff < BeatRange)
			{
				touchedNote.SetActive(false);
				Debug.Log(" success.");
			}
			else
			{
				touchedNote.SetActive(false);
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
		}
		else
		{
			Debug.Log("through");
		}
	}
}