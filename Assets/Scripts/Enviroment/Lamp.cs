using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
	public bool Emission;
	public Light Light;

	private void Start()
	{
		if (Emission)
		{
			if (Random.Range(0f, 1f) < 0.25f)
			{
				GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
				Light.enabled = false;
				return;
			}

			GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
			GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.8f, 0.7f, 0.65f));
			Light.enabled = true;
		}
		else
		{
			GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
			Light.enabled = false;
		}
	}
}
