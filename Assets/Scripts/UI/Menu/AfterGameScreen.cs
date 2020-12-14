using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterGameScreen : MonoBehaviour
{
	private bool lockInput = true;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		StartCoroutine(UnlockInput());
	}

	private void Update()
	{
		if (lockInput)
			return;

		CheckForAnyKey();
	}

	private void CheckForAnyKey()
	{
		if (Input.anyKey)
		{
			StartCoroutine(Routine());

			IEnumerator Routine()
			{
				GetComponent<ScreenFader>().fadeState = ScreenFader.FadeState.In;
				yield return new WaitForSeconds(1f);
				SceneManager.LoadScene("MainMenu2");
			}
		}
	}

	IEnumerator UnlockInput()
	{
		yield return new WaitForSeconds(1.25f);
		lockInput = false;
	}
}
