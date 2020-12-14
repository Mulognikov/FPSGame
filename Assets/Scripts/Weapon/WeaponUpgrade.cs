using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrade : MonoBehaviour
{
	[SerializeField] private int expToUpgrade;

	[HideInInspector] public int upgradedIndex = 0;
	private int weaponExp = 0;
	private bool isUpgraded = false;

	public delegate void WeaponExpCountChangedEventHandler(int current, int needToUpgrade, bool isUpgraded);
	public static event WeaponExpCountChangedEventHandler WeaponExpCountChangedEvent;

	public bool TryUpgradeWeapon(int index)
	{
		if (weaponExp >= expToUpgrade && !isUpgraded)
		{
			UpgradeWeapon(index);
			return true;
		}

		return false;
	}

	private void UpgradeWeapon(int index)
	{
		int i = 0;
		foreach(Transform weapon in transform)
		{
			if (i == index)
			{
				weapon.gameObject.SetActive(true);
			}
			else
			{
				weapon.gameObject.SetActive(false);
			}

			i++;
		}

		isUpgraded = true;
		upgradedIndex = index;

		WeaponExpCountChangedEvent?.Invoke(weaponExp, expToUpgrade, isUpgraded);
	}

	private void OnEnable()
	{
		WeaponExpCountChangedEvent?.Invoke(weaponExp, expToUpgrade, isUpgraded);
	}

	private void Update()
	{
		//if (weaponExp < expToUpgrade)
		//	return;

		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			UpgradeWeapon(1);
		}

		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			UpgradeWeapon(2);
		}
	}

	public void GetExp(int exp)
	{
		weaponExp += exp;
		WeaponExpCountChangedEvent?.Invoke(weaponExp, expToUpgrade, isUpgraded);
	}

	public void InvokeExpChangedEvent()
	{
		WeaponExpCountChangedEvent?.Invoke(weaponExp, expToUpgrade, isUpgraded);
	}
}
