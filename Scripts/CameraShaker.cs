using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
	public static CameraShaker cameraShaker;

	private Camera camera;

	private void Awake()
	{
		cameraShaker = this;
		camera = GetComponent<Camera>();
	}

	public void CameraShake(float vertical, float horizontal)
	{
		Vector3 currentCameraRotation = camera.transform.rotation.eulerAngles;

		if (vertical - currentCameraRotation.x < -90)
		{
			vertical = vertical + (vertical - currentCameraRotation.x + 90.1f);
		}

		camera.transform.rotation = Quaternion.Euler(currentCameraRotation.x - vertical, currentCameraRotation.y + horizontal, currentCameraRotation.z);
	}
}
