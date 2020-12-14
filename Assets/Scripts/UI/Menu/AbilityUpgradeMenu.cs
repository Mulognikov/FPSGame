using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUpgradeMenu : MonoBehaviour
{
	[SerializeField] private GameObject abilityUpgradeMenu;

	public static AbilityUpgradeMenu AUM;

	public delegate void AbilityGotEventHandler();
	public static event AbilityGotEventHandler AbilityGotEvent;

	private bool isOpen;

	private void Awake()
	{
		AUM = this;
		isOpen = false;
	}

	private void Update()
	{
		if (isOpen && Input.GetKeyDown(KeyCode.Escape))
		{
			CloseMenu();
		}
	}

	public void CloseMenu()
	{
		if (isOpen)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			abilityUpgradeMenu.SetActive(false);
			isOpen = false;
			Time.timeScale = 1;
		}
	}

	public void OpenMenu()
	{
		if (!isOpen && Time.timeScale == 1)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			abilityUpgradeMenu.SetActive(true);
			isOpen = true;
			Time.timeScale = 0.0001f;
			ScreenFaderProvider.FadeOut(20000f);
		}
	}

	public void GetAbility(int index)
	{
		PlayerAbility.PA.GetAbility(index);
		AbilityGotEvent?.Invoke();
		CloseMenu();
	}
}
