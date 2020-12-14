using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IHealth
{
	private Player player;
	private Vector3 playerPos;





	[SerializeField] private int health = 30;
	[SerializeField] private int damage = 3;
	[SerializeField] private int viewingRadius = 20;
	[SerializeField] private float rotationSpeed = 1;
	[SerializeField] private float fireRate = 5;
	[SerializeField] private int exp = 50;
	[SerializeField] private Transform endPoint;
	[SerializeField] private Bullet bullet;
	[SerializeField] private GameObject muzzleVFX;
	[SerializeField] private GameObject DeadVFX;
	[SerializeField] private AudioSource laserSound;

	private bool continueAttack = false;
	private bool canFire = true;
	private bool isDead = false;
	private int layerMask = 0b_0010_0000_0000_0000;


	private void Start()
	{
		player = PlayerList.PL.PlayersList[0];
		layerMask = ~layerMask;
		EnemyList.EL.AddTurret(this);
	}

	private void Update()
	{
		if (isDead)
			return;

		float isVisible = IsPlayerVisible();

		if (isVisible == -1f)
		{
			continueAttack = false;
			return;
		}
		else if (isVisible > 0f && isVisible < viewingRadius)
		{
			RotateToPlayer();
			TryAttack();
		}
	}

	private float IsPlayerVisible()
	{
		playerPos = player.transform.position + Vector3.up;

		if (Vector3.Distance(playerPos, transform.position) > viewingRadius)
			return -1f;

		RaycastHit hit;

		if (Physics.Linecast(transform.position, playerPos, out hit, layerMask))
		{
			Debug.DrawLine(transform.position, playerPos, Color.magenta);

			if (hit.transform.gameObject.tag == "Player")
			{
				return Vector3.Distance(transform.position, player.transform.position);
			}
		}

		return -1f;
	}

	private void RotateToPlayer()
	{
		Vector3 direction = player.transform.position - transform.position + Vector3.up;
		Quaternion rotation = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
	}

	private void TryAttack()
	{
		RaycastHit hit;

		if (Physics.Raycast(endPoint.position, endPoint.forward, out hit, viewingRadius, layerMask))
		{
			if (hit.transform.gameObject.tag == "Player")
			{
				Attack();
				return;
			}
		}

		if (continueAttack)
		{
			Attack();
			CallForHelp();

			if (Vector3.Angle(transform.position - player.transform.position, transform.forward) > 90)
			{
				continueAttack = false;
			}
		}
	}

	private void Attack()
	{
		continueAttack = true;

		if (canFire)
			StartCoroutine(Routine());

		IEnumerator Routine()
		{
			laserSound.PlayOneShot(laserSound.clip);
			canFire = false;
			Instantiate(bullet, endPoint.position, endPoint.rotation).Damage = damage;
			Instantiate(muzzleVFX, endPoint.position, endPoint.rotation);
			yield return new WaitForSeconds(1 / fireRate);
			canFire = true;
		}
	}

	public void GetDamage(int damage)
	{
		if (isDead)
			return;

		health -= damage;
		if (health <= 0)
		{
			isDead = true;
			Instantiate(DeadVFX, transform.position - new Vector3(0, 0.25f), Quaternion.identity);
			player.playerWeapon.GetCurrentWeaponUpgrade().GetExp(exp);

			//StartCoroutine(Routine());

			//IEnumerator Routine()
			//{
			//	yield return new WaitForSeconds(0.25f);
			//	EnemyList.EL.Turrets.Remove(this);
			//}
		}
	}

	private void CallForHelp()
	{
		foreach (Enemy enemy in EnemyList.EL.Enemies)
		{
			if (enemy != this && Vector3.Distance(transform.position, enemy.transform.position) < 7f)
			{
				enemy.GoToHelp();
			}
		}
	}
}
