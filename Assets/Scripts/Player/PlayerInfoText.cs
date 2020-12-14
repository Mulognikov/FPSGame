using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoText
{
	public delegate void InfoTextChangedEventHandler(string text, float time);
	public static event InfoTextChangedEventHandler InfoTextChangedEvent;

	public static void SetInfoText(string text, float time)
	{
		InfoTextChangedEvent(text, time);
	}
}
