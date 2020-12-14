using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorQualityCtrl : MonoBehaviour
{
	[SerializeField] private int minimumQualitySetting;
	[SerializeField] private Projector projector;

	private Transform player;
	private int quality;

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
		player = PlayerList.PL.PlayersList[0].transform;
	}

	private void Update()
	{
		if (quality < minimumQualitySetting)
			return;

		if (Vector3.Distance(player.position, transform.position) > quality * 5 + 10)
		{
			projector.enabled = false;
		}
		else
		{
			projector.enabled = true;
		}
	}

	private void QualityChanged(int quality)
	{
		if (quality >= minimumQualitySetting)
		{
			projector.enabled = true;
		}
		else
		{
			projector.enabled = false;
		}

		this.quality = quality;
	}
}
