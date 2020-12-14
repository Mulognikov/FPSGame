using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
	[SerializeField] private float timeToDestroy;

	private void Start()
	{
		Destroy(gameObject, timeToDestroy);
	}

	private void OnDisable()
	{
		Destroy(gameObject, 0f);
	}
}
