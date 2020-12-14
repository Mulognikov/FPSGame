using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFireGuide : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		PlayerInfoText.SetInfoText("[Левая кнопка мыши] Выстрелить", 5f);
		gameObject.SetActive(false);
	}
}
