using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainMenuEnemy : MonoBehaviour
{
	[SerializeField] private Transform startPoint;
	[SerializeField] private Transform endPoint;
	[SerializeField] private float chance;
	[SerializeField] private float speed;

	private bool cooldown = false;
	private NavMeshAgent agent;
	private Animator animator;

	private void Start()
	{
		animator = GetComponentInChildren<Animator>();
		agent = GetComponent<NavMeshAgent>();
		agent.speed = speed;
		animator.SetBool("Run Forward", true);
		animator.speed = 1;
	}

	private void Update()
	{
		if (Random.Range(0f, 100f) < chance)
		{
			if (agent.hasPath || cooldown)
			{
				return;
			}

			transform.position = startPoint.position;
			agent.SetDestination(endPoint.position);
			StartCoroutine(Cooldown(12));
		}
	}

	IEnumerator Cooldown(int time)
	{
		cooldown = true;
		yield return new WaitForSeconds(time);
		cooldown = false;
	}




}
