using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixFontSize : MonoBehaviour
{
	[SerializeField] private Text badText;
	[SerializeField] private Text targetText;

	private void Update()
	{
		badText.fontSize = targetText.cachedTextGenerator.fontSizeUsedForBestFit;
	}
}
