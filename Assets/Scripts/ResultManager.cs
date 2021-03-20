using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    GameObject title;
    GameObject score;
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
        this.goodCount = GameObject.Find("GoodCount");
        this.missCount = GameObject.Find("MissCount");
        this.albumCover = GameObject.Find("AlbumCover");
        this.rank = GameObject.Find("Rank");

        // TODO: 以下の変数は本来ゲーム画面などの別の画面から値を受け取る
        string title = "君が代";
        int goodCount = 100;
        int missCount = 100;
        int score = 1000000;
        string albumCoverImgPath = "hata_kokki_flag_japan";

        this.title.GetComponent<Text>().text = title;
        this.score.GetComponent<Text>().text = score.ToString();
        this.goodCount.GetComponent<Text>().text = goodCount.ToString();
        this.missCount.GetComponent<Text>().text = missCount.ToString();
        this.albumCover.GetComponent<Image>().sprite = Resources.Load<Sprite>(albumCoverImgPath);
        this.rank.GetComponent<Text>().text = this.getRankText(score);
    }

    public void BackToStartScene()
    {
        SceneManager.LoadScene("StartScene");
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
