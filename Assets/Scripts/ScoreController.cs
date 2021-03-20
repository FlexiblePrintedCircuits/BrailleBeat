using System;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
	public event EventHandler onChanged;

	public int Combo { get; private set; } = 0;
	public int MaxCombo { get; private set; } = 0;

	public int Score { get; private set; } = 0;

	public int Good { get; private set; } = 0;

	public int Miss { get; private set; } = 0;

	public void UpdateScore()
	{
		MaxCombo = Math.Max(MaxCombo, Combo);
	}

	public void Success()
	{
		Combo++;
		Good++;
		calcScore();

		onChanged.Invoke(this, EventArgs.Empty);
	}

	public void Failure()
	{
		UpdateScore();
		Combo = 0;
		Miss++;

		onChanged.Invoke(this, EventArgs.Empty);
	}

	void calcScore()
	{
		// TODO: 計算式を適用する
		// 100万点 / ノーツ数が1ノーツ当たりの得点
		Score += 100;
	}
}