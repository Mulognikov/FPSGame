using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertLamp : MonoBehaviour
{
	[SerializeField] private GameObject lights;
	[SerializeField] private float speed;
	[SerializeField] private bool emission;

	private void Start()
	{
		if (emission)
		{
			GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
		}
		else
		{
			GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
		}

		lights.SetActive(emission);

	}

	private void Update()
	{
		lights.transform.Rotate(Vector3.up, speed * Time.deltaTime);
	}
}
