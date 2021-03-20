using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
	[SerializeField] GameObject particle;


	public void Tap()
	{
		var effect = Instantiate(particle, transform);
		effect.GetComponent<ParticleSystem>().Play();
	}
}
