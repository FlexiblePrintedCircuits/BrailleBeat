using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;


public class DotController : MonoBehaviour
{

	float Distance;
	float During;

	Vector3 firstPos;
	bool isGo;
	float GoTime;

	public float Timing { get; set; }
	public bool IsFrame { get; set; } = false;

	void OnEnable()
	{
		isGo = false;
		firstPos = this.transform.position;

		this.UpdateAsObservable()
		  .Where(_ => isGo)
		  .Subscribe(_ =>
		  {
			  this.gameObject.transform.position = new Vector3(firstPos.x - Distance * (Time.time * 1000 - GoTime) / During, firstPos.y, firstPos.z);
		  });
	}

	public void go(float distance, float during)
	{
		Distance = distance;
		During = during;
		GoTime = Time.time * 1000;

		isGo = true;
	}
}