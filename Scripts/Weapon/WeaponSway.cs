using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
	public float amount = 0.01f;
	public float maxAmount = 0.05f;
	public float smoothAmount = 5f;

	private Vector3 startPosition;
	private Quaternion startRotation;

	private void Start()
	{
		startPosition = transform.localPosition;
		startRotation = transform.localRotation;
	}

	private void Update()
	{
		float movementX = Input.GetAxis("Mouse X") * amount;
		float movementY = Input.GetAxis("Mouse Y") * amount;

		movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
		movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);

		Vector3 targetPosition = new Vector3(movementX, movementY, 0);
		transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition + startPosition, Time.deltaTime * smoothAmount);

		if (transform.localRotation != startRotation)
		{
			float k = Quaternion.Angle(transform.localRotation, startRotation);
			k = Mathf.Clamp(k, 0, 5);
			transform.localRotation = Quaternion.Lerp(transform.localRotation, startRotation, Time.deltaTime * k);
		}
	}
}
