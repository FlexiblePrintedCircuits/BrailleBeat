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

	[SerializeField] Button Play;

	[SerializeField] GameObject Tenji;
	[SerializeField] Sprite frameSprite;

	[SerializeField] Transform Center;
	[SerializeField] Transform BeatPoint;

	string Title;
	int BPM;
	List<GameObject> Notes;

	float PlayTime;
	float Distance;
	float During;
	bool isPlaying;
	int noteIndex;

	void OnEnable()
	{

		// setup
		Distance = Math.Abs(BeatPoint.position.x - Center.position.x);
		During = 2 * 1000;
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
		  .Where(_ => Notes.Count > noteIndex)
		  .Where(_ => Notes[noteIndex].GetComponent<DotController>().Timing <= ((Time.time * 1000 - PlayTime) + During))
		  .Subscribe(_ =>
		  {
			  Notes[noteIndex].GetComponent<DotController>().go(Distance, During);
			  noteIndex++;
		  });
	}

	void loadChart()
	{
		Notes = new List<GameObject>();

		string jsonText = Resources.Load<TextAsset>(FilePath).ToString();

		JsonNode json = JsonNode.Parse(jsonText);
		Title = json["title"].Get<string>();
		BPM = int.Parse(json["bpm"].Get<string>());

		foreach (var tenji in json["notes"])
		{
			var type = tenji["type"];
			int count = type.Count;

			var timingMs = int.Parse(tenji["timing"].Get<string>());
			for (var y = 0; y < count; y++)
			{
				var rows = type[y];
				for (var x = 0; x < rows.Count; x++)
				{
					var note = int.Parse(rows[x].Get<string>());

					var padding = new Vector3(x - 1, y - 1) * DotPadding;

					GameObject Dot;
					Dot = Instantiate(Tenji, Center.position + padding, Quaternion.identity);
					if (note == 0)
					{
						Dot.GetComponent<SpriteRenderer>().sprite = frameSprite;
					}

					Dot.GetComponent<DotController>().Timing = timingMs;
					Notes.Add(Dot);
				}
			}

		}
	}

	void play()
	{
		PlayTime = Time.time * 1000;
		isPlaying = true;
		Debug.Log("Game Start!");
	}
}