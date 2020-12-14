using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthReg : MonoBehaviour, IAction
{
	public string ActionText { get; set; }

	[SerializeField] private int minHealth;
	[SerializeField] private int maxHealth;
	[SerializeField] private Color healthEmissionColor;

	private int health;
	private ItemPickUpAnimation itemPickUpAnimation;
	private bool isTaken = false;

	private void Start()
	{
		itemPickUpAnimation = GetComponent<ItemPickUpAnimation>();
		health = Random.Range(minHealth, maxHealth + 1);
		ActionText = "Восстановить здоровье (" + health + " ОЗ)";
		GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", healthEmissionColor);
	}

	private void Update()
	{
		if (isTaken)
			return;
	}

	public void Action()
	{
		if (PlayerList.PL.PlayersList[0].playerHealth.GetHealth() == 100)
		{
			PlayerInfoText.SetInfoText("Вы полностью здоровы", 3f);
			return;
		}

		itemPickUpAnimation.PickUpItem();
		PlayerList.PL.PlayersList[0].playerHealth.RestoreHealth(health);
		isTaken = true;
	}
}
