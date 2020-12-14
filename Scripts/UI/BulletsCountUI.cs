using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletsCountUI : MonoBehaviour
{
	[SerializeField] private Text bulletsCountText;

	private void Start()
	{
		WeaponVer2.BulletsCountChanged += SetBulletsOnUI;
	}

	private void OnDisable()
	{
		WeaponVer2.BulletsCountChanged -= SetBulletsOnUI;
	}

	private void SetBulletsOnUI(int bulletsInMagazine, int AllBullets, PlayerWeaponVer2.AmmoType ammoType)
	{
		bulletsCountText.text = bulletsInMagazine + " / " + AllBullets;
	}
}
