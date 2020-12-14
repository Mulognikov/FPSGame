using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradeUI : MonoBehaviour
{
	public static WeaponUpgradeUI WUUI;

	[Header("Main References")]
	[SerializeField] private GameObject upgradeMenu;
	[SerializeField] private Transform weaponUIPos;
	[SerializeField] private Transform selectedWeaponUIPos;
	[SerializeField] private Transform weaponUIParent;
	[SerializeField] private Text upgradeButtonText;
	[SerializeField] private Text weaponExpText;

	[Header("Weapon Props Text Refernces")]
	[SerializeField] private Text damageText;
	[SerializeField] private Text fireRateText;
	[SerializeField] private Text accuracyText;
	[SerializeField] private Text bulletsCountText;
	[SerializeField] private Text magazineCapacityText;
	[SerializeField] private Text reloadTimeText;
	[SerializeField] private Text recoilText;
	[SerializeField] private Text descriptionText;

	private bool isOpen = false;
	private Player player;
	private List<WeaponVer2> loadedWeapons = new List<WeaponVer2>();
	private GameObject selectedWeapon;
	private int selectedIndex = 0;
	private int currentExp;
	private int expToUpgrade;
	private WeaponUpgrade WU;

	private void Awake()
	{
		WUUI = this;
	}

	private void Start()
	{
		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			yield return new WaitForSeconds(0.25f);
			player = PlayerList.PL.PlayersList[0];
		}
	}

	private void OnEnable()
	{
		WeaponUpgrade.WeaponExpCountChangedEvent += ExpChanged;
		if (player != null)
		{
			player.playerWeapon.GetCurrentWeaponUpgrade().InvokeExpChangedEvent();
		}
	}

	private void OnDisable()
	{
		WeaponUpgrade.WeaponExpCountChangedEvent -= ExpChanged;
	}

	private void Update()
	{
		if (Input.GetKeyDown(PlayerInput.UpgradeWeapon) || Input.GetKeyDown(KeyCode.Joystick1Button8))
		{
			if (isOpen)
			{
				CloseMenu();
			}
			else
			{
				OpenMenu();
			}
		}

		if (isOpen && Input.GetKeyDown(KeyCode.Escape))
		{
			CloseMenu();
		}
	}

	public void UpgradeWeapon(int index)
	{
		if (player.playerWeapon.GetCurrentWeaponUpgrade().TryUpgradeWeapon(index))
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
			upgradeMenu.SetActive(false);
			isOpen = false;
			Time.timeScale = 1;

			foreach (Transform weaponOnUI in weaponUIParent)
			{
				Destroy(weaponOnUI.gameObject);
			}

			Destroy(selectedWeapon.gameObject);
			loadedWeapons.Clear();
		}
	}

	private void OpenMenu()
	{
		if (!isOpen && Time.timeScale == 1)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			upgradeMenu.SetActive(true);
			isOpen = true;
			Time.timeScale = 0.0001f;
			ScreenFaderProvider.FadeOut(20000f);
			LoadWeaponOnUI();
			ShowSelectedWeapon(WU.upgradedIndex);
		}
	}

	private void LoadWeaponOnUI()
	{
		WU = player.playerWeapon.GetCurrentWeaponUpgrade();
		Transform weapons = WU.transform;

		for (int i = 0; i < weapons.childCount; i++)
		{
			GameObject weaponGO = Instantiate(weapons.GetChild(i).gameObject, weaponUIPos.GetChild(i).transform.position, weaponUIPos.GetChild(i).transform.rotation, weaponUIParent);
			WeaponVer2 weapon = weaponGO.GetComponent<WeaponVer2>();
			weaponGO.SetActive(true);
			weapon.IsDropped = true;
			loadedWeapons.Add(weapon);
		}
	}

	public void ShowSelectedWeapon(int index)
	{
		if (loadedWeapons.Count - 1 < index)
			return;

		if (selectedWeapon != null)
			Destroy(selectedWeapon);

		selectedIndex = index;
		selectedWeapon = Instantiate(loadedWeapons[index].gameObject, selectedWeaponUIPos.position, selectedWeaponUIPos.rotation, selectedWeaponUIPos);
		WeaponVer2 weapon = selectedWeapon.GetComponent<WeaponVer2>();
		selectedWeapon.SetActive(true);
		weapon.IsDropped = true;
		BoxCollider colider = selectedWeapon.GetComponentInChildren<BoxCollider>();
		selectedWeapon.transform.position = new Vector3(selectedWeapon.transform.position.x - colider.center.z * 2, selectedWeapon.transform.position.y, selectedWeapon.transform.position.z);

		damageText.text = "Урон: " + loadedWeapons[index].Damage.ToString() + " ед.";
		fireRateText.text = "Скорострельность: " + loadedWeapons[index].FireRate.ToString() + " в/с";
		accuracyText.text = "Разброс: " + loadedWeapons[index].Accuracy.ToString() + " гр.";
		bulletsCountText.text = "Количество пуль: " + loadedWeapons[index].BulletsCount.ToString() + " шт.";
		magazineCapacityText.text = "Магазин: " + loadedWeapons[index].MagazineCapacity.ToString() + " пт.";
		reloadTimeText.text = "Перезрядка: " + loadedWeapons[index].ReloadTime.ToString() + " сек.";
		recoilText.text = "Отдача: " + loadedWeapons[index].Recoil.ToString() + " гр.";
		descriptionText.text = loadedWeapons[index].Description;


		if (selectedIndex == WU.upgradedIndex)
		{
			upgradeButtonText.text = "Уже имеется";
			upgradeButtonText.GetComponentInParent<Button>().interactable = false;
		}
		else
		{
			if (currentExp >= expToUpgrade && WU.upgradedIndex == 0)
			{
				upgradeButtonText.text = "Улучшить";
				upgradeButtonText.GetComponentInParent<Button>().interactable = true;
			}
			else
			{
				upgradeButtonText.text = "Недоступно";
				upgradeButtonText.GetComponentInParent<Button>().interactable = false;
			}
		}
	}

	public void TryUpgradeSelectedWeapon()
	{
		bool result = player.playerWeapon.GetCurrentWeaponUpgrade().TryUpgradeWeapon(selectedIndex);
		if (result)
		{
			CloseMenu();
		}
			
	}

	private void ExpChanged(int currentExp, int expToUpgrade, bool isUpgraded)
	{
		this.currentExp = currentExp;
		this.expToUpgrade = expToUpgrade;

		if (isUpgraded)
		{
			weaponExpText.text = "Оружие улучшено";
		}
		else if (currentExp >= expToUpgrade)
		{
			weaponExpText.text = "Доступно улучшение";

		}
		else
		{
			weaponExpText.text = "Опыт: " + currentExp + " / " + expToUpgrade;
		}
	}
}
