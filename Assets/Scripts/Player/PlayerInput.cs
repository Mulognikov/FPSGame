using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInput
{
	private static string horizontalAxis = "Horizontal";
	private static string verticalAxis = "Vertical";
	private static string mouseX = "Mouse X";
	private static string mouseY = "Mouse Y";
	private static float mouseSensitivity = 150;

	private static KeyCode jump = KeyCode.Space;
	private static KeyCode run = KeyCode.LeftShift;
	private static KeyCode duck = KeyCode.LeftControl;

	private static KeyCode fire = KeyCode.Mouse0;
	private static KeyCode reload = KeyCode.R;
	private static KeyCode action = KeyCode.E;
	private static KeyCode upgradeWeapon = KeyCode.J;
	private static KeyCode flashlight = KeyCode.F;
	private static KeyCode pause = KeyCode.Escape;

	public static KeyCode[] InventoryButtons = new KeyCode[10]
	{
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5,
		KeyCode.Alpha6,
		KeyCode.Alpha7,
		KeyCode.Alpha8,
		KeyCode.Alpha9,
		KeyCode.Alpha0
	};

	private static int graphicsQuality = 3;
	private static int volume = 100;

	public static string HorizontalAxis
	{
		get { return horizontalAxis; }
		set { horizontalAxis = value; SaveSettings(); }
	}

	public static string VerticalAxis
	{
		get { return verticalAxis; }
		set { verticalAxis = value; SaveSettings(); }
	}

	public static string MouseX
	{
		get { return mouseX; }
		set { mouseX = value; SaveSettings(); }
	}

	public static string MouseY
	{
		get { return mouseY; }
		set { mouseY = value; SaveSettings(); }
	}

	public static float MouseSensitivity
	{
		get { return mouseSensitivity; }
		set { mouseSensitivity = value; SaveSettings(); }
	}

	public static KeyCode Jump
	{
		get { return jump; }
		set { jump = value; SaveSettings(); }
	}

	public static KeyCode Run
	{
		get { return run; }
		set { run = value; SaveSettings(); }
	}

	public static KeyCode Duck
	{
		get { return duck; }
		set { duck = value; SaveSettings(); }
	}

	public static KeyCode Fire
	{
		get { return fire; }
		set { fire = value; SaveSettings(); }
	}

	public static KeyCode Reload
	{
		get { return reload; }
		set { reload = value; SaveSettings(); }
	}

	public static KeyCode Action
	{
		get { return action; }
		set { action = value; SaveSettings(); }
	}

	public static KeyCode UpgradeWeapon
	{
		get { return upgradeWeapon; }
		set { upgradeWeapon = value; SaveSettings(); }
	}

	public static KeyCode Flashlight
	{
		get { return flashlight; }
		set { flashlight = value; SaveSettings(); }
	}

	public static KeyCode Pause
	{
		get { return pause; }
		set { pause = value; SaveSettings(); }
	}

	public static int GraphicsQuality
	{
		get { return graphicsQuality; }
		set
		{
			QualitySettings.SetQualityLevel(value - 1);

			graphicsQuality = value;
		}
	}

	public static int Volume
	{
		get { return volume; }
		set { volume = value; SaveSettings(); }
	}

	public static bool SaveSettings()
	{
		try
		{
			PlayerPrefs.SetString("HorizontalAxis", horizontalAxis);
			PlayerPrefs.SetString("VerticalAxis", verticalAxis);
			PlayerPrefs.SetString("MouseX", mouseX);
			PlayerPrefs.SetString("MouseY", mouseY);

			PlayerPrefs.SetFloat("MouseSensitivity", mouseSensitivity);

			PlayerPrefs.SetInt("Jump", (int)jump);
			PlayerPrefs.SetInt("Run", (int)run);
			PlayerPrefs.SetInt("Duck", (int)duck);

			PlayerPrefs.SetInt("Fire", (int)fire);
			PlayerPrefs.SetInt("Reload", (int)reload);
			PlayerPrefs.SetInt("Action", (int)action);
			PlayerPrefs.SetInt("UpgradeWeapon", (int)upgradeWeapon);
			PlayerPrefs.SetInt("Flashlight", (int)flashlight);
			PlayerPrefs.SetInt("Pause", (int)pause);

			PlayerPrefs.SetInt("GraphicsQuality", graphicsQuality);
			PlayerPrefs.SetInt("Volume", volume);

			PlayerPrefs.Save();
		}
		catch
		{
			return false;
		}

		return true;
	}

	public static bool LoadSettings()
	{
		try
		{
			horizontalAxis = PlayerPrefs.GetString("HorizontalAxis", "Horizontal");
			verticalAxis = PlayerPrefs.GetString("VerticalAxis", "Vertical");
			mouseX = PlayerPrefs.GetString("MouseX", "Mouse X");
			mouseY = PlayerPrefs.GetString("MouseY", "Mouse Y");

			mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 150);

			jump = (KeyCode)PlayerPrefs.GetInt("Jump", 32);
			run = (KeyCode)PlayerPrefs.GetInt("Run", 304);
			duck = (KeyCode)PlayerPrefs.GetInt("Duck", 306);

			fire = (KeyCode)PlayerPrefs.GetInt("Fire", 323);
			reload = (KeyCode)PlayerPrefs.GetInt("Reload", 114);
			action = (KeyCode)PlayerPrefs.GetInt("Action", 101);
			upgradeWeapon = (KeyCode)PlayerPrefs.GetInt("UpgradeWeapon", 106);
			flashlight = (KeyCode)PlayerPrefs.GetInt("Flashlight", 102);
			pause = (KeyCode)PlayerPrefs.GetInt("Pause", 27);

			graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", 3);
			volume = PlayerPrefs.GetInt("Volume", 100);
		}
		catch
		{
			return false;
		}

		return true;
	}
}
