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
				var button = Instantiate(this.button, transform.position + padding, Quaternion.identity);
				var index = x + y * 3;
				button.name = $"Button{index}";
				button.transform.parent = transform;
				button.GetComponent<ButtonState>().Index = index;
			}
		}
	}
}
