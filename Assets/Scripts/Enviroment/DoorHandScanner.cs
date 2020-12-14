using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandScanner : MonoBehaviour, IAction
{
	public string ActionText { get; set; }

	[SerializeField] private DoorOpen door;

	private bool isOpen;
	private bool IsOpen
	{
		get { return isOpen; }
		set
		{
			isOpen = value;
			if (value)
			{
				ActionText = "Закрыть дверь";
			}
			else
			{
				ActionText = "Открыть дверь";
			}
		}
	}

	private void Awake()
	{
		IsOpen = false;
	}

	private void Start()
	{
		foreach(DoorOpen badDoor in FindObjectsOfType<DoorOpen>())
		{
			if (badDoor.gameObject == door.gameObject)
			{
				continue;
			}

			if (Vector3.Distance(door.gameObject.transform.position, badDoor.gameObject.transform.position) < 1f)
			{
				badDoor.gameObject.SetActive(false);
				Debug.LogError("YEEESSSS");
			}
		}
	}

	public void Action()
	{
		if (isOpen)
		{
			door.CloseDoor();
			IsOpen = false;
		}
		else
		{
			door.OpenDoor();
			IsOpen = true;
		}
	}
}
