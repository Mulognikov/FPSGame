using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadBobbing : MonoBehaviour
{
	private Vector3 normalPosition;

	private void Start()
	{
		normalPosition = transform.localPosition;
	}

	private void Update()
	{
		transform.localPosition = normalPosition - new Vector3(0, Mathf.Sin(transform.position.magnitude) / 20f);
	}
}
