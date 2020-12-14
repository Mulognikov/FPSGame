using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour, IAction
{
	[SerializeField] private PlayerWeaponVer2.AmmoType ammoType;
	[SerializeField] private int minAmmoCount = 15;
	[SerializeField] public int maxAmmoCount = 25;
	[SerializeField] private Color ammoEmissionColor;

	public string ActionText { get; set; }

	private ItemPickUpAnimation itemPickUpAnimation;
	private bool isTaken = false;
	private int layerMask = 1 << 12;
	private int ammoCount;

	private void Start()
	{
		itemPickUpAnimation = GetComponent<ItemPickUpAnimation>();
		layerMask = ~layerMask;
		GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", ammoEmissionColor);
		ammoCount = UnityEngine.Random.Range(minAmmoCount, maxAmmoCount + 1);
		ActionText = "Подобрать патроны (" + ammoCount + " шт.)";
	}

	private void Update()
	{
		if (isTaken)
			return;

		//SearchPlayer();
	}

	public void Action()
	{
		if (PlayerList.PL.PlayersList[0].playerWeapon[ammoType] == PlayerList.PL.PlayersList[0].playerWeapon.maxPlayerAmmo[ammoType])
		{
			PlayerInfoText.SetInfoText("Достигнуто максимально количество патронов", 3f);
			return;
		}

		itemPickUpAnimation.PickUpItem();
		PlayerList.PL.PlayersList[0].playerWeapon[ammoType] += ammoCount;
		isTaken = true;
	}

	private void SearchPlayer()
	{
		RaycastHit hit;

		if (Physics.Linecast(transform.position, PlayerList.PL.PlayersList[0].transform.position + Vector3.up, out hit, layerMask))
		{
			if (hit.transform.CompareTag("Player") && hit.distance < 4f)
			{
				itemPickUpAnimation.PickUpItem();
				PlayerList.PL.PlayersList[0].playerWeapon[ammoType] += ammoCount;
				isTaken = true;
			}
		}
	}
}
