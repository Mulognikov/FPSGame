using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponVer2 : MonoBehaviour
{
	public static PlayerWeaponVer2 PW;

	public delegate void SwitchStartEventHandler();
	public delegate void SwitchEndEventHandler();
	public delegate void BulletsCountChangedHundler(int allBullets, AmmoType ammoType);

	public static event SwitchStartEventHandler SwitchStartEvent;
	public static event SwitchEndEventHandler SwitchEndEvent;
	public static event BulletsCountChangedHundler BulletsCountChanged;

	public enum AmmoType
	{
		Common,
		Rare,
		Secret
	}

	[SerializeField] private int commonAmmo;
	[SerializeField] private int rareAmmo;
	[SerializeField] private int secretAmmo;

	public int maxCommonAmmo = 60;
	public int maxRareAmmo = 20;
	public int maxSecretAmmo = 20;

	private Dictionary<AmmoType, int> playerAmmo = new Dictionary<AmmoType, int>(3);
	public readonly Dictionary<AmmoType, int> maxPlayerAmmo = new Dictionary<AmmoType, int>(3);

	public int this[AmmoType Key]
	{
		get { return playerAmmo[Key]; }
		set
		{
			if (value > maxPlayerAmmo[Key])
			{
				playerAmmo[Key] = maxPlayerAmmo[Key];
			}
			else
			{
				playerAmmo[Key] = value;
			}

			BulletsCountChanged?.Invoke(playerAmmo[Key], Key);
		}
	}

	private int currentWeapon;

	private void Awake()
	{
		PW = this;

		playerAmmo[AmmoType.Common] = commonAmmo;
		playerAmmo[AmmoType.Rare]   = rareAmmo;
		playerAmmo[AmmoType.Secret] = secretAmmo;

		maxPlayerAmmo[AmmoType.Common] = maxCommonAmmo;
		maxPlayerAmmo[AmmoType.Rare] = maxRareAmmo;
		maxPlayerAmmo[AmmoType.Secret] = maxSecretAmmo;
	}

	private void Start()
	{
		foreach (Transform weapon in transform)
		{
			weapon.gameObject.SetActive(false);
		}

		SwitchWeapon(0);
	}

	private void Update()
	{
		if (Time.timeScale != 1)
			return;

		SwitchWeaponInput();
	}

	private void SwitchWeaponInput()
	{
		for (int i = 0; i < 10; i++)
		{
			if (Input.GetKey(PlayerInput.InventoryButtons[i]))
			{
				TrySwitchWeapon(i);
			}
		}
	}

	private void TrySwitchWeapon(int index)
	{
		if (currentWeapon == index)
			return;

		if (index > transform.childCount - 1 || index < 0)
			return;

		SwitchStartEvent?.Invoke();
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
				SwitchEndEvent?.Invoke();
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
		playerAmmo[type] = Mathf.Abs(bullets);
	}

	public WeaponVer2 GetCurrentWeapon()
	{
		return transform.GetChild(currentWeapon).GetComponentInChildren<WeaponVer2>();
	}

	public WeaponUpgrade GetCurrentWeaponUpgrade()
	{
		return transform.GetChild(currentWeapon).GetComponent<WeaponUpgrade>();
	}
}
