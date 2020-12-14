using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemy : MonoBehaviour, IHealth
{
	[SerializeField] private Animator animator;
	[SerializeField] private GameObject blood;

	[SerializeField] private AudioClip[] hitSounds;
	[SerializeField] private AudioSource audioSource;

	private int i = 0;
	private bool isDead = false;
	private bool anim = false;
	private bool guide = true;

	private void Update()
	{
		if (isDead)
			return;

		if (Random.Range(0f, 1f) < 0.02f && !anim)
		{
			StartCoroutine(SetAnim());
		}
	}

	private void OnEnable()
	{
		RunGuide.GuideCompltedEvent += DisableGuide;
	}

	private void OnDisable()
	{
		RunGuide.GuideCompltedEvent -= DisableGuide;
	}

	private IEnumerator SetAnim()
	{
		anim = true;
		animator.SetTrigger("Idle");
		animator.speed = 3f;
		yield return new WaitForSeconds(1f);
		animator.speed = 1f;
		anim = false;
	}

	public void GetDamage(int damage)
	{
		SpawnBlood();

		if (isDead)
			return;

		isDead = true;

		if (!audioSource.isPlaying)
		{
			audioSource.pitch = Random.Range(0.95f, 1.05f);
			audioSource.PlayOneShot(hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)]);
		}

		animator.speed = 1f;
		animator.SetTrigger("Die");


		audioSource.PlayOneShot(hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)]);
		enabled = false;

		if (guide)
			PlayerInfoText.SetInfoText("[R] Перезарядить оружие", 5f);
	}

	private void SpawnBlood()
	{
		Quaternion rotation = Quaternion.LookRotation(Vector3.forward - Vector3.up);
		Instantiate(blood, transform.position + Vector3.up, rotation);
	}

	private void DisableGuide()
	{
		guide = false;
	}
}
