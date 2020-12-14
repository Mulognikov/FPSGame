using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBullet : Bullet
{
	[SerializeField] private GameObject deathLight;

	private bool isExloded = false;

	private void Start()
	{
		Destroy(gameObject, 3f);
	}

	private void Update()
	{
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("IgnoreBullets"))
		{
			return;
		}

		if (collision.transform.CompareTag(targetTag))
		{
			if (collision.transform.gameObject.TryGetComponent(out IHealth health))
			{
				//health.GetDamage(Damage);
				Destroy(gameObject);
				Explosion();
			}
		}

		if (!collision.transform.CompareTag(ignoreTag))
		{
			Destroy(gameObject);

			if (collision.transform.gameObject.TryGetComponent(out Rigidbody rigidbody))
			{
				rigidbody.AddForce(transform.forward * hitForce, ForceMode.Impulse);
			}
		}
	}

	private void Explosion()
	{
		if (isExloded)
			return;

		isExloded = true;

		foreach (Enemy e in EnemyList.EL.Enemies)
		{
			float distance = Vector3.Distance(transform.position, e.transform.position);

			if (distance < 6)
			{
				e.GetDamage((int)(Damage / Mathf.Pow(distance / 3f, 2)));
				//e.GetDamage(Damage);
			}
		}

		foreach (Turret t in EnemyList.EL.Turrets)
		{
			float distance = Vector3.Distance(transform.position, t.transform.position);

			if (distance < 5)
			{
				t.GetDamage((int)(Damage / Mathf.Pow(distance / 2f, 2)));
			}
		}

		float plyaerDistance = Vector3.Distance(PlayerList.PL.PlayersList[0].transform.position, transform.position) + 1.5f;

		if (plyaerDistance < 5f)
		{
			PlayerList.PL.PlayersList[0].playerHealth.GetDamage((int)(Damage / plyaerDistance));
		}
	}

	private void OnDestroy()
	{
		var d = Instantiate(deathLight, transform.position, transform.rotation);
		var h = Instantiate(hitVFX, transform.position, transform.rotation);
		Destroy(d, 1.5f);
		Destroy(h, 3f);
		Explosion();
	}
}
