using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashlight : MonoBehaviour
{
	[SerializeField] private GameObject flashlight;
	[SerializeField] private GameObject pivot;
	private bool state = false;

	private void Update()
	{
		transform.rotation = pivot.transform.rotation;

		if (Input.GetKeyDown(PlayerInput.Flashlight))
		{
			if (state)
			{
				flashlight.SetActive(false);
				state = false;
			}
			else
			{
				flashlight.SetActive(true);
				state = true;
			}
		}
	}
}
