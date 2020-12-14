using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponExpUI : MonoBehaviour
{
	[SerializeField] private Text weaponExpText;

	private bool showMessage = true;

	private void OnEnable()
	{
		WeaponUpgrade.WeaponExpCountChangedEvent += SetWeaponExpOnUI;
	}

	private void OnDisable()
	{
		WeaponUpgrade.WeaponExpCountChangedEvent -= SetWeaponExpOnUI;
	}

	private void SetWeaponExpOnUI(int current, int needToUpgrade, bool isUpgraded)
	{
		if (isUpgraded)
		{
			weaponExpText.text = "Улучшено";
		}
		else if (current >= needToUpgrade)
		{
			weaponExpText.text = "<color=yellow>Доступно улучшение</color>";

			if (showMessage)
			{
				PlayerInfoText.SetInfoText("[ J ] Открыть меню улучшения оружия", 5f);
				showMessage = false;
			}

		}
		else
		{
			weaponExpText.text = "Опыт: " + current + " / " + needToUpgrade;
		}
	}
}
