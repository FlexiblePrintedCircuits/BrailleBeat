using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVizualize : MonoBehaviour
{
	AudioSpectrum spectrum;
	public Transform[] cubes;
	public float scale;

	void OnEnable()
	{
		spectrum = gameObject.AddComponent<AudioSpectrum>();
	}

	private void Update()
	{
		for (int i = 0; i < cubes.Length; i++)
		{
			var cube = cubes[i];
			var localScale = cube.localScale;
			localScale.y = spectrum.Levels[i] * scale;
			cube.localScale = localScale;
		}
	}
}
