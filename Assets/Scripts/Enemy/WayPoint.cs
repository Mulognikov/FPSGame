﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
	private void Start()
	{
		WayAndChillPoints.WCP.AddWayPoint(transform);
	}
}