using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
	[SerializeField] private Slider mouseSensSlider;
	[SerializeField] private Text mouseSensValueText;
	[SerializeField] private GameObject mainMenu;

	private void Start()
	{
		//mouseSensSlider.value = PlayerInput.MouseSensitivity / 50;
		//mouseSensValueText.text = (PlayerInput.MouseSensitivity / 50).ToString();
	}

	private void OnEnable()
	{
		ScreenFaderProvider.FadeOut(2f);
	}

	public void SliderMove()
	{
		//float sens = ((int)(mouseSensSlider.value * 10)) / 10f;
		//mouseSensValueText.text = sens.ToString();
		//PlayerInput.MouseSensitivity = (int)sens * 50;
	}

	public void BackButton()
	{
		if (PlayerInput.SaveSettings())
		{
			Debug.Log("Settings saved succefully");
		}
		else
		{
			Debug.Log("Saving settings failed");
		}

		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			ScreenFaderProvider.FadeIn(2f);
			yield return new WaitForSeconds(0.5f);
			mainMenu.SetActive(true);
			gameObject.SetActive(false);
		}
	}
}
