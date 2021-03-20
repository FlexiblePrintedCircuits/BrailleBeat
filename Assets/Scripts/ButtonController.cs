using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
	readonly KeyCode[] keys = new KeyCode[] { KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Q, KeyCode.W, KeyCode.E };
	public event EventHandler<int> OnButtonDown;
	public event EventHandler<int> OnButtonUp;

	ButtonState state;

	[SerializeField] Sprite UpSprite;
	[SerializeField] Sprite DownSprite;

	private void OnEnable()
	{
		state = gameObject.AddComponent<ButtonState>();

		// タップ判定
		GetComponent<ObservableEventTrigger>()
				.OnPointerDownAsObservable()
				.Subscribe((_) => OnButtonDown.Invoke(this, state.Index));

		GetComponent<ObservableEventTrigger>()
			.OnPointerUpAsObservable()
			.Subscribe(_ => OnButtonUp.Invoke(this, state.Index));

		// キーボード判定
		this.UpdateAsObservable()
			.Where(_ => Input.GetKeyDown(keys[state.Index]))
			.Subscribe(_ => OnButtonDown.Invoke(this, state.Index));

		this.UpdateAsObservable()
			.Where(_ => Input.GetKeyUp(keys[state.Index]))
			.Subscribe(_ => OnButtonUp.Invoke(this, state.Index));

		OnButtonDown += ButtonController_OnButtonDown;
		OnButtonUp += ButtonController_OnButtonUp;
	}

	private void ButtonController_OnButtonUp(object sender, int e)
	{
		state.Pressed = false;

		GetComponent<SpriteRenderer>().sprite = UpSprite;
	}

	private void ButtonController_OnButtonDown(object sender, int e)
	{
		state.Pressed = true;

		GetComponent<SpriteRenderer>().sprite = DownSprite;
	}
}
