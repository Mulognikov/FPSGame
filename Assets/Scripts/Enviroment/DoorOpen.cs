using DunGen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
	[SerializeField] private GameObject Door;
	[SerializeField] private Door dunGenDoor;
	[SerializeField] private bool openByTrigger = true;
	[SerializeField] private AudioClip[] doorSounds;
	[SerializeField] private AudioSource audioSource;

	private Vector3 noramlDoorPosition;
	private int nearObjectsCount = 0;

	private void Start()
	{
		noramlDoorPosition = Door.transform.position;

		foreach (DoorOpen badDoor in FindObjectsOfType<DoorOpen>())
		{
			if (badDoor.gameObject == gameObject || badDoor.openByTrigger == false)
			{
				continue;
			}

			if (Vector3.Distance(gameObject.transform.position, badDoor.transform.position) < 0.25f)
			{
				badDoor.gameObject.SetActive(false);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!openByTrigger)
			return;

		if (other.CompareTag("Player") || other.CompareTag("Enemy"))
		{
			nearObjectsCount++;

			if (nearObjectsCount == 1)
			{
				OpenDoor();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!openByTrigger)
			return;

		if (other.CompareTag("Player") || other.CompareTag("Enemy"))
		{
			StartCoroutine(Routine());
			IEnumerator Routine()
			{
				yield return new WaitForSeconds(0.1f);
				nearObjectsCount--;
				if (nearObjectsCount == 0)
				{
					CloseDoor();
				}
			}
		}
	}

	public void OpenDoor()
	{
		Vector3 target = Door.transform.position + Vector3.up * 3;
		dunGenDoor.IsOpen = true;

		PlayDoorSound();

		StopAllCoroutines();
		StartCoroutine(Open());

		IEnumerator Open()
		{
			while (Vector3.Distance(Door.transform.position, target) > 0.01f)
			{
				Door.transform.position = Vector3.Lerp(Door.transform.position, target, Time.deltaTime * 3);
				yield return null;
			}
		}
	}

	public void CloseDoor()
	{
		Vector3 target = noramlDoorPosition;

		PlayDoorSound();

		StopAllCoroutines();
		StartCoroutine(Close());

		IEnumerator Close()
		{
			while (Vector3.Distance(Door.transform.position, target) > 0.01f)
			{
				Door.transform.position = Vector3.Lerp(Door.transform.position, target, Time.deltaTime * 3);
				yield return null;
			}
		}

		dunGenDoor.IsOpen = false;
	}

	private void PlayDoorSound()
	{
		audioSource.pitch = Random.Range(0.8f, 1.2f);
		audioSource.PlayOneShot(doorSounds[Random.Range(0, doorSounds.Length)]);
	}
}
