using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTimer : MonoBehaviour
{
	public static PlayerTimer PT;

	public delegate void PlayerTimeChangedEventHandler(int time);
	public static event PlayerTimeChangedEventHandler PlayerTimeChangedEvent;

	[SerializeField] private int allTime;
	[SerializeField] private int deathTime = 60;

	private void Awake()
	{
		PT = this;
	}

	private void OnEnable()
	{
		PlayerHealth.PlayerDeadEvent += PlayerDead;
	}

	private void OnDisable()
	{
		PlayerHealth.PlayerDeadEvent -= PlayerDead;
	}

	private void Start()
	{
		StartCoroutine(DecTime());
		PlayerTimeChangedEvent?.Invoke(allTime);
	}

	IEnumerator DecTime()
	{
		while (allTime > 0)
		{
			yield return new WaitForSeconds(1f);
			if (Time.timeScale == 1)
			{
				allTime--;
			}
			PlayerTimeChangedEvent?.Invoke(allTime);
		}

		Lose();
	}

	private void Lose()
	{
		ScreenFader screenFader = PlayerList.PL.PlayersList[0].GetComponent<ScreenFader>();

		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			ScreenFaderProvider.FadeIn(1f);
			yield return new WaitForSeconds(1f);
			SceneManager.LoadScene("LoseScene");
		}
	}

	private void PlayerDead()
	{
		allTime -= deathTime;
	}

	public int GetTimeLeft()
	{
		return allTime;
	}

}
