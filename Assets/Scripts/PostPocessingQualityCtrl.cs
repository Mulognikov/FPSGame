using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostPocessingQualityCtrl : MonoBehaviour
{
	[SerializeField] private int minimumQualitySetting;
	[SerializeField] private GameObject postProcessing;

	private void OnEnable()
	{
		PauseMenu.QualityChangedEvent += QualityChanged;
	}

	private void OnDisable()
	{
		PauseMenu.QualityChangedEvent -= QualityChanged;
	}

	private void Start()
	{
		QualityChanged(PlayerInput.GraphicsQuality);	
	}

	private void QualityChanged(int quality)
	{
		if (quality >= minimumQualitySetting)
		{
			postProcessing.SetActive(true);
		}
		else
		{
			postProcessing.SetActive(false);
		}
	}
}
