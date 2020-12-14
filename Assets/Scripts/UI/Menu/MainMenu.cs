using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	private void OnEnable()
	{
		ScreenFaderProvider.FadeOut(1f);
	}

	private void Awake()
	{
		if (PlayerInput.LoadSettings())
		{
			Debug.Log("Settings loaded succefully");
		}
		else
		{
			Debug.Log("Loading settings failed");
		}

		ScreenFaderProvider.FadeOut(1f);
	}

	private void Start()
	{
		ScreenFaderProvider.FadeOut(1f);
	}
}
