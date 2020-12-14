using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BombPlant : MonoBehaviour, IAction
{
	public string ActionText { get; set; }

	[SerializeField] private int bombCountNeed = 3;


	public delegate void AllBombsPlantedEventHandler();
	public static event AllBombsPlantedEventHandler AllBombsPlantedEvent;

	private int installedBombs = 0;

	private void Start()
	{
		ActionText = "Установить заряд";
	}

	public void Action()
	{
		TryPlantBomb();
	}

	private void TryPlantBomb()
	{
		if (PlayerList.PL.PlayersList[0].BombCount > 0)
		{
			PlantBomb();
		}
		else
		{
			PlayerInfoText.SetInfoText("Нет зарядов", 3f);
		}
	}

	private void PlantBomb()
	{
		installedBombs += PlayerList.PL.PlayersList[0].BombCount;
		PlayerList.PL.PlayersList[0].BombCount = 0;

		PlayerInfoText.SetInfoText("Заряд установлен (" + installedBombs + " / " + bombCountNeed + ")", 3f);
		PlayerObjective.PO.BombPlanted(installedBombs);

		if (installedBombs == bombCountNeed)
		{
			AllBombsPlantedEvent?.Invoke();
		}
	}
}
