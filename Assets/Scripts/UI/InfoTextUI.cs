using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoTextUI : MonoBehaviour
{
	[SerializeField] private Text infoText;

	private void OnEnable()
	{
		PlayerInfoText.InfoTextChangedEvent += SetInfoTextOnUI;
	}

	private void OnDisable()
	{
		PlayerInfoText.InfoTextChangedEvent -= SetInfoTextOnUI;
	}

	private void Start()
	{
		infoText.text = "";
		SetInfoTextOnUI("[WASD] Управление игровым персонажем", 7f);
	}

	private void SetInfoTextOnUI(string text, float time)
	{
		infoText.text = text;
		StopAllCoroutines();
		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			yield return new WaitForSeconds(time);
			infoText.text = "";
		}
	}
}
