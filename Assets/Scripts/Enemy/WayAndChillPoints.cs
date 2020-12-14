using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayAndChillPoints : MonoBehaviour
{
	public static WayAndChillPoints WCP;

	private List<Transform> wayPointsList = new List<Transform>();

	private void Awake()
	{
		WCP = this;
	}

	public void AddWayPoint(Transform point)
	{
		wayPointsList.Add(point);
	}

	public Transform GetRandomWayPoint()
	{
		return wayPointsList[Random.Range(0, wayPointsList.Count - 1)];
	}
}
