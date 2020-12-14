using DunGen;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingQualityCtrl : MonoBehaviour
{
	[SerializeField] private BasicRoomCullingCamera cullingObject;

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
		switch (quality)
		{
			case 1:
				cullingObject.AdjacentTileDepth = 2;
				break;
			case 2:
				cullingObject.AdjacentTileDepth = 3;
				break;
			case 3:
				cullingObject.AdjacentTileDepth = 3;
				break;
			case 4:
				cullingObject.AdjacentTileDepth = 4;
				break;
			case 5:	
				cullingObject.AdjacentTileDepth = 4;
				break;
			case 6:
				cullingObject.AdjacentTileDepth = 5;
				break;
		}
	}
}
