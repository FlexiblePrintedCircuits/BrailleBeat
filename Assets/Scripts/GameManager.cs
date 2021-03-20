using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class GameManager : MonoBehaviour
{

	[SerializeField] string FilePath;

	[SerializeField] Button Play;
	[SerializeField] Button SetChart;

	[SerializeField] GameObject Tenji;

	[SerializeField] Transform Center;
	[SerializeField] Transform BeatPoint;

	string Title;
	int BPM;
	List<GameObject> Notes;

	void OnEnable()
	{

		SetChart.onClick
		  .AsObservable()
		  .Subscribe(_ => loadChart());
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

			for (var y = 0; y < count; y++)
			{
				var rows = type[y];
				for (var x = 0; x < rows.Count; x++)
				{
					var note = int.Parse(rows[x].Get<string>());

					if (note != 0)
					{
						var padding = new Vector3(x - 1, y - 1) * .5f;

						GameObject Note;
						Note = Instantiate(Tenji, Center.position + padding, Quaternion.identity);
						Notes.Add(Note);
					}
				}
			}

		}
	}

	void play()
	{
		Debug.Log("Game Start!");
	}
}