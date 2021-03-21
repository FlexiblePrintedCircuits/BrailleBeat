using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
	readonly KeyCode[] keys = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Z, KeyCode.X, KeyCode.C };
	public event EventHandler<int> OnButtonDown;
	public event EventHandler<int> OnButtonUp;

	ButtonState state;

	[SerializeField] Sprite UpSprite;
	[SerializeField] Sprite DownSprite;
	[SerializeField] GameObject buttonEffect;
	GameObject effect;

	private void OnEnable()
	{
		state = gameObject.AddComponent<ButtonState>();
		effect = Instantiate(buttonEffect, transform);
		effect.GetComponent<ParticleSystem>().Stop();

		// �^�b�v����
		var eventTrigger = GetComponent<ObservableEventTrigger>();
		eventTrigger.OnPointerDownAsObservable()
			.Subscribe((_) => OnButtonDown.Invoke(this, state.Index));

		eventTrigger
			.OnPointerUpAsObservable()
			.Subscribe(_ => OnButtonUp.Invoke(this, state.Index));

		// �L�[�{�[�h����
		this.UpdateAsObservable()
			.Where(_ => Input.GetKeyDown(keys[state.Index]))
			.Subscribe(_ => OnButtonDown.Invoke(this, state.Index));

		this.UpdateAsObservable()
			.Where(_ => Input.GetKeyUp(keys[state.Index]))
			.Subscribe(_ => OnButtonUp.Invoke(this, state.Index));

		OnButtonDown += ButtonController_OnButtonDown;
		OnButtonUp += ButtonController_OnButtonUp;
	}

	public void PlaySuccessEffect()
	{
		effect.GetComponent<ParticleSystem>().Play();
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
