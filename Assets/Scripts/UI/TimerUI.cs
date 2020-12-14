using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
	[SerializeField] private Text timerText;

	private void OnEnable()
	{
		PlayerTimer.PlayerTimeChangedEvent += SetTimerOnUI;
	}

	private void OnDisable()
	{
		PlayerTimer.PlayerTimeChangedEvent -= SetTimerOnUI;
	}

	private void SetTimerOnUI(int time)
	{
		int minutes = time / 60;
		int seconds = time - minutes * 60;
		string secondsString;

		if (seconds < 10)
		{
			secondsString = "0" + seconds;
		}
		else
		{
			secondsString = seconds.ToString();
		}


		timerText.text = "Время: " + minutes + ":" + secondsString;
	}
}
