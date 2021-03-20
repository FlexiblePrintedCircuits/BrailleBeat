using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatButtonBuilder : MonoBehaviour
{
	[SerializeField] GameObject button;

	void OnEnable()
	{
		for (int x = 0; x < 3; x++)
		{
			for (int y = 0; y < 3; y++)
			{
				var padding = new Vector3(x - 1, y - 1) * 2f;
				var point = Instantiate(button, transform.position + padding, Quaternion.identity);
				point.transform.parent = transform;
			}
		}
	}
}
