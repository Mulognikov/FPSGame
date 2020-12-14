using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnPoint : MonoBehaviour
{
	public static Transform RespawnPoint;

	private void Start()
	{
		RespawnPoint = transform;
	}
}
