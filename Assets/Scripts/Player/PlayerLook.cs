using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
	public static PlayerLook PL;

	[SerializeField] private Transform playerBody;
	[SerializeField] private Camera playerCamera;

	private float xAxisClamp;

	public void Awake()
	{
		Application.targetFrameRate = 240;
		PL = this;

		LockCursor();
		xAxisClamp = 0.0f;
	}

	private void Start()
	{
#if UNITY_EDITOR
		//UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
#endif
	}

	private void LockCursor()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		if (Time.timeScale != 1)
			return;

		CameraRotation();
	}

	private void CameraRotation()
	{
		float mouseX = Input.GetAxis(PlayerInput.MouseX) * PlayerInput.MouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis(PlayerInput.MouseY) * PlayerInput.MouseSensitivity * Time.deltaTime;

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
		//transform.Rotate(Vector3.left * mouseY);

		//cameraPivot.transform.Rotate(Vector3.left * mouseY);
		//transform.position = cameraPivot.position + cameraPivot.transform.forward * horizontal + cameraPivot.transform.up * vertical;

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

				transform.Rotate(Vector3.right * vertical * Time.timeScale);


				yield return new WaitForEndOfFrame();
			}
		}
	}

	private float EasingSmoothSquared(float x)
	{
		return x < 0.5 ? x*x*2 : (1-(1-x)*(1-x)*2);
	}
}
