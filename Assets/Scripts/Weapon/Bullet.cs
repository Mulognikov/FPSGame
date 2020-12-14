using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[HideInInspector] public int Damage = 0;

	[SerializeField] protected int speed;
	[SerializeField] protected GameObject hitVFX;
	[SerializeField] protected GameObject enemyHitVFX;
	[SerializeField] protected string targetTag;
	[SerializeField] protected string ignoreTag;
	[SerializeField] protected float hitForce = 0;

	private Vector3 lastPos;

	private void Start()
	{
		lastPos = transform.position;
		Destroy(gameObject, 3f);
	}

	private void Update()
	{
		lastPos = transform.position;
		transform.Translate(Vector3.forward * speed * Time.deltaTime);

		RaycastHit hit;

		if(Physics.Linecast(lastPos, transform.position, out hit))
		{
			if (hit.transform.CompareTag("IgnoreBullets"))
			{
				return;
			}

			if (hit.transform.CompareTag(targetTag))
			{
				if (hit.transform.gameObject.TryGetComponent(out IHealth health))
				{
					health.GetDamage(Damage);

					if (enemyHitVFX == null || hit.transform.gameObject.name == "SM_Prop_Turret_Small_Gun_Ceiling_01")
						return;

					var h = Instantiate(enemyHitVFX, hit.point, Quaternion.identity);
					Destroy(h, 3f);
					Destroy(gameObject);
					return;
				}	
			}

			if (!hit.transform.CompareTag(ignoreTag))
			{
				Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
				var h = Instantiate(hitVFX, hit.point, rotation);
				Destroy(h, 3f);
				Destroy(gameObject);

				if (hit.transform.gameObject.TryGetComponent(out Rigidbody rigidbody))
				{
					rigidbody.AddForce(transform.forward * hitForce, ForceMode.Impulse);
				}
			}
		}
	}
}
