using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
	public static PlayerLook PL;

	[SerializeField] private Transform playerBody;

	private float xAxisClamp;
	private Camera playerCamera;
	private PlayerInput PI;

	public void Awake()
	{
		PL = this;

		LockCursor();
		xAxisClamp = 0.0f;

		playerCamera = GetComponent<Camera>();
	}

	private void Start()
	{
		PI = PlayerInput.PI;
	}

	private void LockCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		CameraRotation();
	}

	private void CameraRotation()
	{
		float mouseX = Input.GetAxis(PI.MouseX) * PI.MouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis(PI.MouseY) * PI.MouseSensitivity * Time.deltaTime;

		xAxisClamp += mouseY;

		if (xAxisClamp > 90f)
		{
			xAxisClamp = 90f;
			mouseY = 0f;
			ClampXAxisRotatiocToValue(270f);
		}
		else if (xAxisClamp < -90f)
		{
			xAxisClamp = -90f;
			mouseY = 0f;
			ClampXAxisRotatiocToValue(90f);
		}

		transform.Rotate(Vector3.left * mouseY);
		playerBody.Rotate(Vector3.up * mouseX);

		if (Mathf.Round(Vector3.Angle(transform.forward, playerBody.forward * 100) / 100) != Mathf.Abs(Mathf.Round(xAxisClamp * 100) / 100))
		{
			if (transform.eulerAngles.x < 180)
				xAxisClamp = Vector3.Angle(transform.forward, playerBody.forward) * -1;
			else
				xAxisClamp = Vector3.Angle(transform.forward, playerBody.forward);
		}
	}

	private void ClampXAxisRotatiocToValue(float value)
	{
		Vector3 eulerRotation = transform.eulerAngles;
		eulerRotation.x = value;
		transform.eulerAngles = eulerRotation;
	}

	public void ShakeCameraByRecoil(float vertical, float horizontal)
	{
		xAxisClamp += vertical;
		transform.Rotate(Vector3.left * vertical);
		playerBody.Rotate(Vector3.up * horizontal);

		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			vertical /= 25;
			int i = 0;
			while ( i++ < 100)
			{
				vertical = vertical * 0.95f;
				xAxisClamp += vertical;
				transform.Rotate(Vector3.right * vertical);
				yield return new WaitForEndOfFrame();
			}
		}
	}

	private float EasingSmoothSquared(float x)
	{
		return x < 0.5 ? x*x*2 : (1-(1-x)*(1-x)*2);
	}
}
