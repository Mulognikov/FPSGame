using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IHealth
{
	private Player player;
	private Vector3 playerPos;



	[SerializeField] private int health;
	[SerializeField] private int damage;
	[SerializeField] private int viewingAngle = 180;
	[SerializeField] private int viewingRadius = 20;
	[SerializeField] private int exp = 50;
	[SerializeField] private Animator animator;
	[SerializeField] private GameObject blood;

	[SerializeField] private AudioClip[] hitSounds;
	[SerializeField] private AudioSource audioSource;

	private List<GameObject> playersList = new List<GameObject>();

	private NavMeshAgent agent;
	private int layerMask = 0b_0010_0100_0000_0000;

	public enum EnemyStates
	{
		Idle,
		Walk,
		Run,
		Attack,
		Dead,
		RotateToPlayer
	}

	private EnemyStates enemyState;

	public EnemyStates EnemyState
	{
		get { return enemyState; }
		set
		{
			enemyState = value;

			switch (value)
			{
				case EnemyStates.Attack:
					agent.isStopped = true;
					break;
				case EnemyStates.Dead:
					agent.isStopped = true;
					break;
				case EnemyStates.Run:
					agent.isStopped = false;
					break;
				case EnemyStates.Idle:
					agent.isStopped = false;
					break;
				case EnemyStates.RotateToPlayer:
					agent.isStopped = true;
					break;
				case EnemyStates.Walk:
					agent.isStopped = false;
					break;
			}

		}
	}

	private void Start()
	{
		EnemyList.EL.AddEnemy(this);
		agent = GetComponent<NavMeshAgent>();
		EnemyState = EnemyStates.Idle;
		viewingAngle /= 2;
		layerMask = ~layerMask;


		player = PlayerList.PL.PlayersList[0];
	}

	private void OnEnable()
	{
		WeaponVer2.FireEvent += GoToNoise;
	}

	private void OnDisable()
	{
		WeaponVer2.FireEvent -= GoToNoise;
	}

	private void Update()
	{
		SetAnimation();
		SetPathToPlayer();

		if (EnemyState == EnemyStates.Dead || EnemyState == EnemyStates.Attack)
			return;

		float isVisible = IsPlayerVisible();

		if (isVisible == -1 && enemyState != EnemyStates.Run)
		{
			Walk();
		}
		else if (isVisible > 3f)
		{
			Run();
			CallForHelp();
		}
		else if (isVisible < 3f && isVisible > 0f)
		{
			RotateToPlayer();
		}
		else if (!agent.hasPath)
		{
			Idle();
		}
	}

	public void GetDamage(int damage)
	{
		if (EnemyState != EnemyStates.Dead)
		{
			Run();
			health -= damage;

			if (UnityEngine.Random.Range(0, 1f) < 0.7f)
			{
				if (!audioSource.isPlaying)
				{
					audioSource.pitch = Random.Range(0.95f, 1.05f);
					audioSource.PlayOneShot(hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)]);
				}
			}

			if (health <= 0)
			{
				EnemyState = EnemyStates.Dead;
				animator.SetTrigger("Die");
				player.playerWeapon.GetCurrentWeaponUpgrade().GetExp(exp);
				SpawnBlood();

				audioSource.PlayOneShot(hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)]);

				//StartCoroutine(Routine());

				//IEnumerator Routine()
				//{
				//	yield return new WaitForSeconds(0.25f);
				//	EnemyList.EL.Enemies.Remove(this);
				//}
			}
		}
	}

	private void SetAnimation()
	{
		if (agent.velocity.magnitude > 0.25f)
		{
			if (EnemyState == EnemyStates.Run)
			{
				animator.SetBool("Walk Forward", false);
				animator.SetBool("Run Forward", true);
			}

			if (EnemyState == EnemyStates.Walk)
			{
				animator.SetBool("Run Forward", false);
				animator.SetBool("Walk Forward", true);
			}

		}
		else
		{
			animator.SetBool("Run Forward", false);
			animator.SetBool("Walk Forward", false);
		}
	}

	private float IsPlayerVisible()
	{
		playerPos = player.transform.position + Vector3.up;

		if (Vector3.Distance(playerPos, transform.position) > viewingRadius)
			return -1f;

		RaycastHit hit;

		if (Physics.Linecast(transform.position + transform.forward, playerPos, out hit, layerMask))
		{
			Debug.DrawLine(transform.position + transform.forward, playerPos, Color.red);

			if (hit.transform.gameObject.tag == "Player" )
			{
				if (Vector3.Distance(transform.position, player.transform.position) > 7f)
				{
					if (Vector3.Angle(transform.forward, (transform.position - player.transform.position).normalized) > viewingAngle)
					{
						return Vector3.Distance(transform.position, player.transform.position);
					}
					else
					{
						return -1f;
					}
				}
				else
				{
					return Vector3.Distance(transform.position, player.transform.position);
				}
			}
		}

		return -1f;
	}

	private void Walk()
	{
		EnemyState = EnemyStates.Walk;
		animator.speed = agent.velocity.magnitude;

		if (!agent.hasPath)
		{
			agent.speed = 1;
			agent.acceleration = 50;
			agent.SetDestination(WayAndChillPoints.WCP.GetRandomWayPoint().position);
		}
	}

	private void Run()
	{
		EnemyState = EnemyStates.Run;
		animator.speed = 1;

		agent.speed = 10;
		agent.acceleration = 1000;
		agent.SetDestination(player.transform.position);
	}

	private void Idle()
	{
		EnemyState = EnemyStates.Idle;
		animator.speed = 1;
	}

	private void SetPathToPlayer()
	{
		if (IsPlayerVisible() != -1)
		{
			agent.SetDestination(player.transform.position);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			playersList.Add(other.gameObject);
			if (EnemyState != EnemyStates.Attack)
				StartCoroutine(Attack());
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			playersList.Remove(other.gameObject);
		}
	}

	private void RotateToPlayer()
	{
		EnemyState = EnemyStates.RotateToPlayer;

		Vector3 direction = player.transform.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 5 * Time.deltaTime);
	}

	IEnumerator Attack()
	{
		if (EnemyState != EnemyStates.Dead)
		{
			EnemyState = EnemyStates.Attack;

			while (playersList.Count > 0)
			{
				if (EnemyState == EnemyStates.Dead)
					break;

				animator.SetTrigger("Stab Attack");
							
				yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length / 3);

				if (playersList.Count > 0)
					PlayerHealth.PH.GetDamage(damage);

				yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length / 3 * 2);
			}

			if (EnemyState != EnemyStates.Dead)
			{
				EnemyState = EnemyStates.Idle;
			}
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

	public void GoToHelp()
	{
		if (EnemyState != EnemyStates.Dead && EnemyState != EnemyStates.Attack)
		{
			agent.SetDestination(player.transform.position);
			Run();
		}
	}

	private void GoToNoise()
	{
		if (EnemyState != EnemyStates.Dead && EnemyState != EnemyStates.Attack && EnemyState != EnemyStates.RotateToPlayer && EnemyState != EnemyStates.Run)
		{
			if (Vector3.Distance(transform.position, player.transform.position) < viewingRadius / 2)
			{
				EnemyState = EnemyStates.Walk;

				animator.speed = 2;
				agent.speed = 2;
				agent.acceleration = 50;
				agent.SetDestination(player.transform.position);
			}

		}
	}

	private void SpawnBlood()
	{
		Quaternion rotation = Quaternion.LookRotation(Vector3.forward - Vector3.up);
		Instantiate(blood, transform.position + Vector3.up , rotation);
	}
}
