using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFaderProvider : MonoBehaviour
{
	private static ScreenFader screenFader;

	private void Awake()
	{
		screenFader = GetComponent<ScreenFader>();
	}

	public static void FadeIn(float speed)
	{
		screenFader.fadeSpeed = speed;
		screenFader.fadeState = ScreenFader.FadeState.In;
	}

	public static void FadeOut(float speed)
	{
		screenFader.fadeSpeed = speed;
		screenFader.fadeState = ScreenFader.FadeState.Out;
	}
}
