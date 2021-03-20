using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
	readonly KeyCode[] keys = new KeyCode[] { KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Q, KeyCode.W, KeyCode.E };
	public event EventHandler<int> OnButtonPressed;

	ButtonState state;

	private void OnEnable()
	{
		state = gameObject.AddComponent<ButtonState>();

		GetComponent<ObservableEventTrigger>()
				.OnPointerDownAsObservable()
				.Subscribe((_) => OnButtonPressed.Invoke(this, state.Index));

		// キーボード判定
		this.UpdateAsObservable()
			.Where(_ => Input.GetKeyDown(keys[state.Index]))
			.Subscribe(_ => OnButtonPressed.Invoke(this, state.Index));
	}

}
