using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IAction
{
	[SerializeField] private Color bombEmissionColor;

	public string ActionText { get; set; }

	public delegate void PlayerPickUpTheBombHandler();
	public static PlayerPickUpTheBombHandler PlayerPickUpTheBombEvent;

	private ItemPickUpAnimation itemPickUpAnimation;

	public void Action()
	{
		PlayerList.PL.PlayersList[0].BombCount += 1;
		PlayerObjective.PO.BombFinded();
		PlayerPickUpTheBombEvent?.Invoke();
		itemPickUpAnimation.PickUpItem();
	}

	private void Start()
	{
		ActionText = "Взять заряд";
		itemPickUpAnimation = GetComponent<ItemPickUpAnimation>();
		GetComponentInChildren<Renderer>().material.SetColor("_EmissionColor", bombEmissionColor);
		//GetComponentInChildren<Renderer>().sharedMaterial.EnableKeyword("_EMISSION");
	}
}
