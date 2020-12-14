using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	public static PlayerInput PI;

	public string HorizontalAxis = "Horizontal";
	public string VerticalAxis = "Vertical";
	public string MouseX = "Mouse X";
	public string MouseY = "Mouse Y";
	public float MouseSensitivity = 50;

	public KeyCode Jump = KeyCode.Space;
	public KeyCode Run = KeyCode.LeftShift;
	public KeyCode Duck = KeyCode.LeftControl;

	public KeyCode Fire = KeyCode.Mouse0;
	public KeyCode Reload = KeyCode.R;

	public KeyCode[] InventoryButtons = new KeyCode[10]
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

	private void Awake()
	{
		PI = this;
	}
}
