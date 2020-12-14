using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUpAnimation : MonoBehaviour
{
	[SerializeField] private Vector3 offset = Vector3.zero;

	public void PickUpItem()
	{
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Collider>().enabled = false;
		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			float i = 1;
			Transform playerTransform = PlayerList.PL.PlayersList[0].transform;

			while (Vector3.Distance(transform.position, playerTransform.position + Vector3.up + offset) > 0.25f)
			{
				transform.position = Vector3.Lerp(transform.position, playerTransform.position + Vector3.up + offset, Time.deltaTime * i * Time.timeScale);
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, i * Time.deltaTime * Time.timeScale);
				i *= 1.5f;
				yield return new WaitForEndOfFrame();
			}

			Destroy(gameObject);
		}
	}
}
