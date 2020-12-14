using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityUI : MonoBehaviour
{
	private Text abilityText;

	private void OnEnable()
	{
		PlayerAbilityInvisible.AbilityActiveTimeRemainsEvent += SetAbilityTextOnUIActive;
		PlayerAbilityInvisible.AbilityCooldownTimeRemainsEvent += SetAbilityTextOnUICooldown;
		PlayerAbilityInvisible.AbilityReadyEvent += SetAbilityReadyTextOnUI;

		PlayerAbilityShield.AbilityActiveTimeChangedEvent += SetAbilityTextOnUIActive;
		PlayerAbilityShield.AbilityCooldownTimeChangedEvent += SetAbilityTextOnUICooldown;
		PlayerAbilityShield.AbilityReadyEvent += SetAbilityReadyTextOnUI;

		abilityText = GetComponent<Text>();
	}

	private void OnDisable()
	{
		PlayerAbilityInvisible.AbilityActiveTimeRemainsEvent -= SetAbilityTextOnUIActive;
		PlayerAbilityInvisible.AbilityCooldownTimeRemainsEvent -= SetAbilityTextOnUICooldown;
		PlayerAbilityInvisible.AbilityReadyEvent -= SetAbilityReadyTextOnUI;

		PlayerAbilityShield.AbilityActiveTimeChangedEvent -= SetAbilityTextOnUIActive;
		PlayerAbilityShield.AbilityCooldownTimeChangedEvent -= SetAbilityTextOnUICooldown;
		PlayerAbilityShield.AbilityReadyEvent -= SetAbilityReadyTextOnUI;
	}

	private void Start()
	{
		abilityText.text = "";
	}

	private void SetAbilityTextOnUIActive(float time)
	{
		if (time > 10)
		{
			abilityText.text = "Активно: " + Math.Round(time).ToString();
		}
		else
		{
			if (Math.Round(time, 1) % 1 == 0)
			{
				abilityText.text = "Активно: " + Math.Round(time, 1).ToString() + ",0";
			}
			else
			{
				abilityText.text = "Активно: " + Math.Round(time, 1).ToString();
			}
		}
	}

	private void SetAbilityTextOnUICooldown(float time)
	{
		if (time > 10)
		{
			abilityText.text = "Перезарядка: " + Math.Round(time).ToString();
		}
		else
		{
			if (Math.Round(time, 1) % 1 == 0)
			{
				abilityText.text = "Перезарядка: " + Math.Round(time, 1).ToString() + ",0";
			}
			else
			{
				abilityText.text = "Перезарядка: " + Math.Round(time, 1).ToString();
			}
		}
	}

	private void SetAbilityReadyTextOnUI()
	{
		abilityText.text = "[Q] Способность";
	}
}
