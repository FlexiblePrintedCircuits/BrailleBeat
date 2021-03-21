using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatPointBuilder : MonoBehaviour
{
	[SerializeField] GameManager manager;
	[SerializeField] GameObject beatPoint;

	void OnEnable()
	{
		for (int x = 0; x < 3; x++)
		{
			for (int y = 0; y < 3; y++)
			{
				var i = x + y * 3;
				var padding = new Vector3(x - 1, -1 * (y - 1)) * manager.DotPadding;
				var point = Instantiate(beatPoint, transform.position + padding, Quaternion.identity);
				point.name = $"BeatButton{i}";
				point.transform.parent = transform;
			}
		}


	}
}
