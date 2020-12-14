using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
	public static PlayerAbility PA;

	private void Awake()
	{
		PA = this;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			GetAbility(0);
		}

		if (Input.GetKeyDown(KeyCode.F2))
		{
			GetAbility(1);
		}
	}

	public void GetAbility(int index)
	{
		int i = 0;

		foreach(Transform t in transform)
		{
			if (index == i)
			{
				t.gameObject.SetActive(true);
			}
			else
			{
				t.gameObject.SetActive(false);
			}

			i++;
		}
	}

}
