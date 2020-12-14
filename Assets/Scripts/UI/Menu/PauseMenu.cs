using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] private GameObject pauseMenu;
	[SerializeField] private GameObject pauseMenuButtons;
	[SerializeField] private GameObject pauseSettings;

	[SerializeField] private Slider mouseSensSlider;
	[SerializeField] private Text mouseSensValueText;

	[SerializeField] private Slider graphicsSlider;
	[SerializeField] private Text graphicsValueText;

	[SerializeField] private Slider audioSlider;
	[SerializeField] private Text audioValueText;
	[SerializeField] private AudioMixerGroup mixer;

	private bool isOpen = false;

	public delegate void QualityChangedEventHandler(int quality);
	public static event QualityChangedEventHandler QualityChangedEvent;

	private void Start()
	{
		mouseSensSlider.value = PlayerInput.MouseSensitivity / 50;
		mouseSensValueText.text = (PlayerInput.MouseSensitivity / 50).ToString();

		graphicsSlider.value = QualitySettings.GetQualityLevel() + 1;
		graphicsValueText.text = SetGraphicsText(QualitySettings.GetQualityLevel() + 1);

		audioSlider.value = PlayerInput.Volume;
		audioValueText.text = PlayerInput.Volume.ToString();
		mixer.audioMixer.SetFloat("Volume", Mathf.Lerp(-80, 0, Mathf.Pow(audioSlider.value / 100, 0.4f)));
	}

	private void Update()
	{
		if (Input.GetKeyDown(PlayerInput.Pause) || Input.GetKeyDown(KeyCode.Joystick1Button9))
		{
			if (isOpen)
			{
				CloseMenu();
			}
			else
			{
				OpenMenu();
			}
		}
	}

	public void CloseMenu()
	{
		if (isOpen)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			pauseMenu.SetActive(false);
			pauseMenuButtons.SetActive(true);
			pauseSettings.SetActive(false);
			isOpen = false;
			Time.timeScale = 1;
		}
	}

	private void OpenMenu()
	{
		if (!isOpen && Time.timeScale == 1)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			pauseMenu.SetActive(true);
			isOpen = true;
			Time.timeScale = 0.0001f;
			ScreenFaderProvider.FadeOut(20000f);
		}
	}

	public void GoToMainMenu()
	{
		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			Time.timeScale = 0.999f;
			ScreenFaderProvider.FadeIn(1f);
			yield return new WaitForSeconds(1.1f);
			Time.timeScale = 1f;
			SceneManager.LoadScene("MainMenu2");
		}
	}

	public void Settings()
	{
		pauseMenuButtons.SetActive(false);
		pauseSettings.SetActive(true);
	}

	public void Back()
	{
		if (PlayerInput.SaveSettings())
		{
			Debug.Log("Settings saved succefully");
		}
		else
		{
			Debug.Log("Saving settings failed");
		}

		pauseMenuButtons.SetActive(true);
		pauseSettings.SetActive(false);
	}

	public void Play()
	{
		StartCoroutine(Routine());
		StartCoroutine(FadeSound());

		IEnumerator Routine()
		{
			ScreenFaderProvider.FadeIn(1f);
			yield return new WaitForSeconds(1.1f);
			SceneManager.LoadScene("DunGenTestScene");
		}

		IEnumerator FadeSound()
		{
			AudioSource audio = GetComponentInChildren<AudioSource>();
			while (true)
			{
				audio.volume = Mathf.Lerp(0, audio.volume, 0.9f);
				yield return null;
			}
		}
	}

	public void SettingsMainMenu()
	{
		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			ScreenFaderProvider.FadeIn(3f);
			yield return new WaitForSeconds(0.35f);
			pauseMenuButtons.SetActive(false);
			pauseSettings.SetActive(true);
			ScreenFaderProvider.FadeOut(3f);
		}
	}

	public void BackButtonMainMenu()
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
			ScreenFaderProvider.FadeIn(3f);
			yield return new WaitForSeconds(0.35f);
			pauseMenuButtons.SetActive(true);
			pauseSettings.SetActive(false);
			ScreenFaderProvider.FadeOut(3f);
		}
	}

	public void Quit()
	{
		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			ScreenFaderProvider.FadeIn(1f);
			yield return new WaitForSeconds(1f);
			Application.Quit();
		}
	}

	public void SliderMove()
	{
		float sens = ((int)(mouseSensSlider.value * 10)) / 10f;
		mouseSensValueText.text = sens.ToString();
		PlayerInput.MouseSensitivity = sens * 50;
	}

	public void GraphicsSliderMove()
	{
		PlayerInput.GraphicsQuality = (int)graphicsSlider.value;
		graphicsValueText.text = SetGraphicsText(PlayerInput.GraphicsQuality);
		QualityChangedEvent?.Invoke(PlayerInput.GraphicsQuality);
	}

	private string SetGraphicsText(int qualityLevel)
	{
		switch (qualityLevel)
		{
			case 1: return "Очень низкое";
			case 2: return "Низкое";
			case 3: return "Среднее";
			case 4: return "Высокое";
			case 5: return "Очень высокое";
			case 6: return "Максимальное";
		}
		
		return "Среднее";
	}

	public void AudioSliderMove()
	{
		mixer.audioMixer.SetFloat("Volume", Mathf.Lerp(-80, 0, Mathf.Pow(audioSlider.value / 100, 0.4f)));
		audioValueText.text = audioSlider.value.ToString();
		PlayerInput.Volume = (int)audioSlider.value;
	}
}
