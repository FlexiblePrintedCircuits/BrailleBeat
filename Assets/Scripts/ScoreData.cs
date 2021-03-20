using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreData : MonoBehaviour
{
    public readonly static ScoreData Instance = new ScoreData();
    public int score = 0;
    public int goodCount = 0;
    public int missCount = 0;
    public string title = "";
    public string imgPath = "";

    void reset()
    {
        this.score = 0;
        this.goodCount = 0;
        this.missCount = 0;
        this.title = "";
        this.imgPath = "";
    }
}
