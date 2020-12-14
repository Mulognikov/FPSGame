using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityShield : MonoBehaviour
{
	[SerializeField] private GameObject shieldPrefab;
	[SerializeField] private float abilityActiveTime = 5;
	[SerializeField] private float abilityCooldownTime = 30;


	public delegate void AbilityActiveTimeChangedEventHandler(float activeTime);
	public delegate void AbilityCooldownTimeChangedEventHandler(float cooldownTime);
	public delegate void AbilityReadyEventHandler();

	public static event AbilityActiveTimeChangedEventHandler AbilityActiveTimeChangedEvent;
	public static event AbilityCooldownTimeChangedEventHandler AbilityCooldownTimeChangedEvent;
	public static event AbilityReadyEventHandler AbilityReadyEvent;


	private float abilityActiveTimeRemains;
	private float abilityCooldownTimeRemains;

	private void OnEnable()
	{
		abilityActiveTimeRemains = 0;
		abilityCooldownTimeRemains = 0;
		AbilityReadyEvent?.Invoke();
	}

	private void Start()
	{
		AbilityReadyEvent?.Invoke();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			BuildShield();
		}
	}

	private void BuildShield()
	{
		if (abilityCooldownTimeRemains > 0 || abilityActiveTimeRemains > 0)
			return;

		GameObject shield = Instantiate(shieldPrefab, PlayerList.PL.PlayersList[0].transform.position, PlayerList.PL.PlayersList[0].transform.rotation);
		Destroy(shield, abilityActiveTime);
		StartCoroutine(ActiveAbilityTime());
	}

	private IEnumerator ActiveAbilityTime()
	{
		abilityActiveTimeRemains = abilityActiveTime;

		while (abilityActiveTimeRemains > 0)
		{
			abilityActiveTimeRemains -= Time.deltaTime;

			if (abilityActiveTimeRemains < 0)
			{
				abilityActiveTimeRemains = 0;
			}

			try
			{
				AbilityActiveTimeChangedEvent?.Invoke(abilityActiveTimeRemains);
			}
			catch { }

			yield return new WaitForEndOfFrame();
		}

		StartCoroutine(AbilityCooldownTime());
	}

	private IEnumerator AbilityCooldownTime()
	{
		abilityActiveTimeRemains = 0;
		abilityCooldownTimeRemains = abilityCooldownTime;

		while (abilityCooldownTimeRemains > 0)
		{
			abilityCooldownTimeRemains -= Time.deltaTime;

			if (abilityCooldownTimeRemains < 0)
			{
				abilityCooldownTimeRemains = 0;
			}

			AbilityCooldownTimeChangedEvent?.Invoke(abilityCooldownTimeRemains);
			yield return new WaitForEndOfFrame();
		}

		AbilityReadyEvent?.Invoke();
	}
}
