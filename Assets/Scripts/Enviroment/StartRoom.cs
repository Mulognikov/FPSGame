using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : MonoBehaviour
{
	[SerializeField] private GameObject[] steamFX;
	[SerializeField] private float steamTime;
	[SerializeField] private DoorOpen entranceDoor;
	[SerializeField] private DoorOpen exitDoor;
	[SerializeField] private DoorHandScanner handScanner;
	[SerializeField] private AudioClip[] audios;

	private bool isWork = true;
	private bool start = true;

	private void OnEnable()
	{
		BombPlant.AllBombsPlantedEvent += EndGameState;
	}

	private void OnDisable()
	{
		BombPlant.AllBombsPlantedEvent -= EndGameState;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (isWork)
			{
				isWork = false;

				if (start)
				{
					handScanner.gameObject.SetActive(false);
					StartCoroutine(SteamStartRoutine());
					entranceDoor.CloseDoor();
				}
				else
				{
					handScanner.gameObject.SetActive(true);
					StartCoroutine(EndRoutine());
					exitDoor.CloseDoor();
				}
			}
		}
	}

	IEnumerator SteamStartRoutine()
	{
		yield return new WaitForSeconds(2f);
		GetComponent<AudioSource>().PlayOneShot(audios[0]);
		PlayerInfoText.SetInfoText("[1] [2] Сменить оружие", 5f);

		foreach (GameObject steam in steamFX)
		{
			steam.SetActive(true);
		}

		yield return new WaitForSeconds(steamTime);

		foreach (GameObject steam in steamFX)
		{
			steam.SetActive(false);
		}

		PlayerInfoText.SetInfoText("[F] Включить фонарик", 4f);

		yield return new WaitForSeconds(2f);

		if (start)
		{
			exitDoor.OpenDoor();
		}
		else
		{
			exitDoor.CloseDoor();
		}

		start = false;
	}

	IEnumerator EndRoutine()
	{
		exitDoor.CloseDoor();
		yield return new WaitForSeconds(1f);
		GetComponent<AudioSource>().PlayOneShot(audios[0]);
		yield return new WaitForSeconds(5f);
		entranceDoor.OpenDoor();
	}

	private void EndGameState()
	{
		isWork = true;
	}
}
