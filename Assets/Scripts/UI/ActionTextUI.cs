using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionTextUI : MonoBehaviour
{
	[SerializeField] private Text actionText;

	private void OnEnable()
	{
		PlayerAction.LookToActionObjectEvent += SetActionTextOnUI;
		actionText.text = "";
	}

	private void OnDisable()
	{
		PlayerAction.LookToActionObjectEvent -= SetActionTextOnUI;
	}

	private void SetActionTextOnUI(string text)
	{
		if (text == "")
		{
			actionText.text = "";
		}
		else
		{
			actionText.text = "[ E ] " + text;
		}
	}
}
