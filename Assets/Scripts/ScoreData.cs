using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreData : MonoBehaviour
{
    public readonly static ScoreData Instance = new ScoreData();
    public string title = "";
    public Sprite img = null;
    public int score = 0;
    public int maxCombo = 0;
    public int goodCount = 0;
    public int missCount = 0;

    public void Reset()
    {
        this.title = "";
        this.img = null;
        this.maxCombo = 0;
        this.score = 0;
        this.goodCount = 0;
        this.missCount = 0;
    }
}
