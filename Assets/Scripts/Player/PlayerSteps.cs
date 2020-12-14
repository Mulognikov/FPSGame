using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSteps : MonoBehaviour
{
	[SerializeField] private AudioSource stepSound;
	[SerializeField] private float stepSize;
	[SerializeField] private float minStepSize;
	[SerializeField] private float minPitch = 0.975f;
	[SerializeField] private float maxPitch = 1.025f;

	private float stepSum = 0;
	private Vector3 lastPos;
	private bool isPlayer;

	private void Awake()
	{
		lastPos = transform.position;
	}

	private void Start()
	{
		if (TryGetComponent<Player>(out Player p))
		{
			isPlayer = true;
		}
		else
		{
			isPlayer = false;
		}
	}

	private void Update()
	{
		TryStep();
	}

	private void TryStep()
	{
		if (isPlayer)
		{
			if (!PlayerList.PL.PlayersList[0].playerMove.IsGrounded())
				return;
		}


		if (Vector3.Distance(transform.position, lastPos) > minStepSize)
			stepSum += Vector3.Distance(transform.position, lastPos);

		if (stepSum >= stepSize)
		{
			stepSound.pitch = Random.Range(minPitch, maxPitch);
			stepSound.PlayOneShot(stepSound.clip);
			stepSum = 0;
		}

		lastPos = transform.position;
	}
}
