using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
	GameObject title;
	GameObject score;
	GameObject maxCombo;
	GameObject goodCount;
	GameObject missCount;
	GameObject albumCover;
	GameObject rank;

	// Start is called before the first frame update
	void Start()
	{
		// リザルト画面を構成するGameObjectを取得
		this.title = GameObject.Find("Title");
		this.score = GameObject.Find("Score");
		this.maxCombo = GameObject.Find("MaxCombo");
		this.goodCount = GameObject.Find("GoodCount");
		this.missCount = GameObject.Find("MissCount");
		this.albumCover = GameObject.Find("AlbumCover");
		this.rank = GameObject.Find("Rank");

		this.title.GetComponent<Text>().text = ScoreData.Instance.title;
		this.score.GetComponent<Text>().text = ScoreData.Instance.score.ToString();
		this.maxCombo.GetComponent<Text>().text = ScoreData.Instance.maxCombo.ToString();
		this.goodCount.GetComponent<Text>().text = ScoreData.Instance.goodCount.ToString();
		this.missCount.GetComponent<Text>().text = ScoreData.Instance.missCount.ToString();
		this.albumCover.GetComponent<Image>().sprite = ScoreData.Instance.img;
		this.rank.GetComponent<Text>().text = this.getRankText(ScoreData.Instance.score);
	}

	public void BackToStartScene()
	{
		// 遷移する前にスコアは初期化しておく
		ScoreData.Instance.Reset();
		FadeManager.Instance.LoadScene("StartScene", 1f);
	}

	private string getRankText(int score)
	{
		if (score >= 900000)
			return "S";
		else if (score >= 800000)
			return "A";
		else if (score >= 700000)
			return "B";
		else if (score >= 700000)
			return "C";
		else if (score >= 600000)
			return "D";
		else
			return "E";
	}
}
