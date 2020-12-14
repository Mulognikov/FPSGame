using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpaceship : MonoBehaviour, IAction
{
	public string ActionText { get; set; }

	private bool canLeave = false;

	private void Awake()
	{
		ActionText = "NO";
	}

	private void OnEnable()
	{
		BombPlant.AllBombsPlantedEvent += AllowLeave;
	}

	private void OnDisable()
	{
		BombPlant.AllBombsPlantedEvent += AllowLeave;
	}

	public void Action()
	{
		if (canLeave)
		{
			PlayerObjective.PO.Leaved();
			GoToWinScene();
		}
	}

	private void AllowLeave()
	{
		canLeave = true;
		ActionText = "Улететь";
	}

	private void GoToWinScene()
	{
		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			Time.timeScale = 0.999f;
			ScreenFaderProvider.FadeIn(1f);
			yield return new WaitForSeconds(1f);
			Time.timeScale = 1f;
			SceneManager.LoadScene("WinScene");
			gameObject.SetActive(false);
		}
	}
}
