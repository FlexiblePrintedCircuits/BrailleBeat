using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    GameObject title;
    GameObject score;
    GameObject hitCount;
    GameObject missCount;
    // Start is called before the first frame update
    void Start()
    {
        // リザルト画面を構成するGameObjectを取得
        this.title = GameObject.Find("Title");
        this.score = GameObject.Find("Score");
        this.hitCount = GameObject.Find("HitCount");
        this.missCount = GameObject.Find("MissCount");

        // TODO: 以下の変数は本来ゲーム画面などの別の画面から値を受け取る
        string title = "君が代";
        int hitCount = 100;
        int missCount = 100;
        int score = 1000000;

        this.title.GetComponent<Text>().text = title;
        this.score.GetComponent<Text>().text = "SCORE: " + score;
        this.hitCount.GetComponent<Text>().text = "HIT: " + hitCount;
        this.missCount.GetComponent<Text>().text = "MISS: " + missCount;
    }
}
