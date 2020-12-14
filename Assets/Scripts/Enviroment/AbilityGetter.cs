using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityGetter : MonoBehaviour, IAction
{
	public string ActionText { get; set; }
	private static bool abilityGot = false;

	private void Awake()
	{
		ActionText = "Получить способность";
	}

	private void OnEnable()
	{
		AbilityUpgradeMenu.AbilityGotEvent += AbilityGot;
	}

	private void OnDisable()
	{
		AbilityUpgradeMenu.AbilityGotEvent -= AbilityGot;
	}

	private void Update()
	{
		if (abilityGot)
		{
			enabled = false;
			ActionText = "NO";
		}
	}

	public void Action()
	{
		if (abilityGot)
			return;

		AbilityUpgradeMenu.AUM.OpenMenu();
	}

	private void AbilityGot()
	{
		abilityGot = true;
		enabled = false;
		ActionText = "NO";
	}
}
