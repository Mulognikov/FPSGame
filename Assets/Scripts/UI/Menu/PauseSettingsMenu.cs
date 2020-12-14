using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseSettingsMenu : MonoBehaviour
{
	[SerializeField] private Slider mouseSensSlider;
	[SerializeField] private Text mouseSensValueText;
	[SerializeField] private GameObject mainMenu;

	private void Start()
	{
		mouseSensSlider.value = PlayerInput.MouseSensitivity / 50;
		mouseSensValueText.text = (PlayerInput.MouseSensitivity / 50).ToString();
	}

	private void OnEnable()
	{

	}

	public void SliderMove()
	{
		float sens = ((int)(mouseSensSlider.value * 10)) / 10f;
		mouseSensValueText.text = sens.ToString();
		PlayerInput.MouseSensitivity = sens * 50;
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
			yield return new WaitForSeconds(0f);
			mainMenu.SetActive(true);
			gameObject.SetActive(false);
		}
	}
}
