using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    GameObject title;
    GameObject score;
    GameObject hitCount;
    GameObject missCount;
    GameObject albumCover;

    // Start is called before the first frame update
    void Start()
    {
        // リザルト画面を構成するGameObjectを取得
        this.title = GameObject.Find("Title");
        this.score = GameObject.Find("Score");
        this.hitCount = GameObject.Find("HitCount");
        this.missCount = GameObject.Find("MissCount");
        this.albumCover = GameObject.Find("AlbumCover");

        // TODO: 以下の変数は本来ゲーム画面などの別の画面から値を受け取る
        string title = "君が代";
        int hitCount = 100;
        int missCount = 100;
        int score = 1000000;
        string albumCoverImgPath = "hata_kokki_flag_japan";

        this.title.GetComponent<Text>().text = title;
        this.score.GetComponent<Text>().text = "SCORE: " + score;
        this.hitCount.GetComponent<Text>().text = "HIT: " + hitCount;
        this.missCount.GetComponent<Text>().text = "MISS: " + missCount;
        this.albumCover.GetComponent<Image>().sprite = Resources.Load<Sprite>(albumCoverImgPath);
    }

    public void BackToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}
