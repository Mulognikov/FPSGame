using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletsCountUI : MonoBehaviour
{
	[SerializeField] private Text bulletsCountText;

	private int currentWeaponBullets;
	private int allBullets;
	private PlayerWeaponVer2.AmmoType currentAmmoType;

	private void OnEnable()
	{
		WeaponVer2.BulletsCountChangedEvent += SetCurrentAndAllBulletsOnUI;
		PlayerWeaponVer2.BulletsCountChanged += SetAllBulletsOnUI;
	}

	private void OnDisable()
	{
		WeaponVer2.BulletsCountChangedEvent -= SetCurrentAndAllBulletsOnUI;
		PlayerWeaponVer2.BulletsCountChanged -= SetAllBulletsOnUI;
	}

	private void SetCurrentAndAllBulletsOnUI(int bulletsInMagazine, int allBullets, PlayerWeaponVer2.AmmoType ammoType)
	{
		currentWeaponBullets = bulletsInMagazine;
		this.allBullets = allBullets;
		currentAmmoType = ammoType;
		bulletsCountText.text = currentWeaponBullets + " / " + this.allBullets;
	}

	private void SetAllBulletsOnUI(int allBullets, PlayerWeaponVer2.AmmoType ammoType)
	{
		if (currentAmmoType == ammoType)
		{
			bulletsCountText.text = currentWeaponBullets + " / " + allBullets;
		}
	}
}
