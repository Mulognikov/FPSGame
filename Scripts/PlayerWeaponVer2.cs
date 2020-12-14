using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponVer2 : MonoBehaviour
{
	public static PlayerWeaponVer2 PlayerWeapon;


	public delegate void SwitchEventHandler(int index);
	public static event SwitchEventHandler SwitchEvent;


	public enum AmmoType
	{
		Common,
		Rare,
		Secret
	}

	[SerializeField] private int commonAmmo;
	[SerializeField] private int rareAmmo;
	[SerializeField] private int secretAmmo;
	[SerializeField] private Transform weaponPosition;

	private Dictionary<AmmoType, int> playerAmmo = new Dictionary<AmmoType, int>(3);
	private int currentWeapon = 0;
	private PlayerInput PI;

	private void Awake()
	{
		PlayerWeapon = this;

		playerAmmo[AmmoType.Common] = commonAmmo;
		playerAmmo[AmmoType.Rare] = rareAmmo;
		playerAmmo[AmmoType.Secret] = secretAmmo;
	}

	private void Start()
	{
		PI = PlayerInput.PI;
	}

	private void Update()
	{
		SwitchWeaponInput();
	}

	private void SwitchWeaponInput()
	{
		for (int i = 0; i < 10; i++)
		{
			if (Input.GetKey(PI.InventoryButtons[i]))
			{
				TrySwitchWeapon(i);
			}
		}
	}

	private void TrySwitchWeapon(int index)
	{
		if (currentWeapon == index)
			return;

		if (index > transform.childCount - 1)
			return;

		SwitchWeapon(index);
	}

	private void SwitchWeapon(int index)
	{
		int i = 0;
		foreach (Transform weapon in transform)
		{
			if (i == index)
			{
				weapon.gameObject.SetActive(true);
				currentWeapon = i;
				SwitchEvent?.Invoke(i);
			}
			else
				weapon.gameObject.SetActive(false);

			i++;
		}
	}

	public int GetBulletsByType(AmmoType ammoType)
	{
		return playerAmmo[ammoType];
	}

	public void SetBulletByType(int bullets, AmmoType type)
	{
		playerAmmo[type] = bullets;
	}
}
