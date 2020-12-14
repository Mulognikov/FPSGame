using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlantFindCtrl : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.CompareTag("Player"))
		{
			PlayerObjective.PO.PlantFinded();
			gameObject.SetActive(false);
		}
	}
}
