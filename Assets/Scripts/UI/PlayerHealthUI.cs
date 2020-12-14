using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
	[SerializeField] private Text PlayerHealthText;

	private void OnEnable()
	{
		PlayerHealth.PlayerHealthChangedEvent += SetPlayerHealthOnUI;
	}

	private void OnDisable()
	{
		PlayerHealth.PlayerHealthChangedEvent -= SetPlayerHealthOnUI;
	}

	private void SetPlayerHealthOnUI(int health)
	{
		PlayerHealthText.text = "\u271a " + health.ToString();
	}
}
