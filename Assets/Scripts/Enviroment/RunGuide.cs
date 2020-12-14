using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunGuide : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		PlayerInfoText.SetInfoText("[Левый Shift] Бег", 5f);
		GuideCompltedEvent?.Invoke();
		gameObject.SetActive(false);
	}

	public delegate void GuideCompltedEventHandler();
	public static event GuideCompltedEventHandler GuideCompltedEvent;
}
