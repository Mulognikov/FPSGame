using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public int speed;
	public GameObject hitVFX;
	private Vector3 lastPos;

	private void Start()
	{
		lastPos = transform.position;
		Destroy(gameObject, 3f);
	}

	private void Update()
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);

		RaycastHit hit;

		if(Physics.Linecast(lastPos, transform.position, out hit))
		{
			if(hit.transform.gameObject.tag != "Player")
			{
				var h = Instantiate(hitVFX, hit.point - new Vector3(-0.05f, -0.05f, -0.05f), Quaternion.identity);
				Destroy(h, 3f);
				Destroy(gameObject);
			}

		}
	}
}
