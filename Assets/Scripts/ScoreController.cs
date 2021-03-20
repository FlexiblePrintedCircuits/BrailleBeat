using System;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
	public event EventHandler onChanged;

	public readonly int MaxPoint = 1000000;

	public int Score { get => (int)(MaxPoint * ((double)Good / NotesCount)); }

	public int NotesCount { get; set; } = 0;

	public int Combo { get; private set; } = 0;

	public int MaxCombo { get; private set; } = 0;

	public int Good { get; private set; } = 0;

	public int Miss { get; private set; } = 0;

	public void UpdateScore()
	{
		MaxCombo = Math.Max(MaxCombo, Combo);
	}

	public void Success()
	{
		Debug.Log("success.");
		Combo++;
		Good++;

		onChanged.Invoke(this, EventArgs.Empty);
	}

	public void Failure()
	{
		Debug.Log("failure.");
		UpdateScore();
		Combo = 0;
		Miss++;

		onChanged.Invoke(this, EventArgs.Empty);
	}
}