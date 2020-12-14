using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpGuide : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		PlayerInfoText.SetInfoText("[Space (Пробел)] Прыжок", 5f);
		gameObject.SetActive(false);
	}
}
