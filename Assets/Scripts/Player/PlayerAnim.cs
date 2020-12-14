using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
	public static PlayerAnim PA;

	[SerializeField] private Animator animator;
	[SerializeField] private Player player;
	[SerializeField] private Transform lookPoint;

	[SerializeField] private Vector3 IKAxisTargetRun;
	[SerializeField] private Vector3 lookPointTargetPosRun;

	[SerializeField] private Vector3 IKAxisTargetReload;
	[SerializeField] private Vector3 lookPointTargetPosReload;

	private Vector3 lookPointNormalPos;
	private Vector3 IKAxisNormal;
	private Coroutine coroutine;

	private void Awake()
	{
		PA = this;
	}

	private void Start()
	{
		lookPointNormalPos = lookPoint.localPosition;
		IKAxisNormal = player.aimIK.solver.axis;
	}

	public void SetAnimationPose(float x, float y)
	{
		animator.SetFloat("x", x);
		animator.SetFloat("y", y);
	}

	public void FireAnimation()
	{
		animator.SetTrigger("Fire");
	}

	public void ReloadAnimation()
	{
		animator.SetTrigger("Reload");

		float lastAnimatorSpeed = animator.speed;

		StartCoroutine(Routine());

		IEnumerator Routine()
		{
			coroutine = StartCoroutine(HandsToTargetRoutine());

			yield return new WaitForSeconds(0.01f);
			animator.speed = animator.speed * animator.GetCurrentAnimatorStateInfo(0).length / player.playerWeapon.GetCurrentWeapon().GetRealodTime();

			yield return new WaitForSeconds(player.playerWeapon.GetCurrentWeapon().GetRealodTime() - player.playerWeapon.GetCurrentWeapon().GetRealodTime() / 3.15f);
			StopCoroutine(coroutine);
			coroutine = null;

			yield return new WaitForSeconds(player.playerWeapon.GetCurrentWeapon().GetRealodTime() / 3.5f);
			animator.speed = lastAnimatorSpeed;
		}

		IEnumerator HandsToTargetRoutine()
		{
			while (true)
			{
				player.aimIK.solver.axis = Vector3.Lerp(player.aimIK.solver.axis, IKAxisTargetReload, 5f * Time.deltaTime);
				lookPoint.localPosition = Vector3.Lerp(lookPoint.localPosition, lookPointTargetPosReload, 5f * Time.deltaTime);

				yield return new WaitForEndOfFrame();
			}
		}
	}

	public void RunAnimation(bool value)
	{
		animator.SetBool("Run", value);

		if (value)
		{
			player.aimIK.solver.axis = Vector3.Lerp(player.aimIK.solver.axis, IKAxisTargetRun, 5f * Time.deltaTime);
			lookPoint.localPosition = Vector3.Lerp(lookPoint.localPosition, lookPointTargetPosRun, 5f * Time.deltaTime);
		}
		else
		{
			if (coroutine != null)
				return;

			player.aimIK.solver.axis = Vector3.Lerp(player.aimIK.solver.axis, IKAxisNormal, 7f * Time.deltaTime);
			lookPoint.localPosition = Vector3.Lerp(lookPoint.localPosition, lookPointNormalPos, 7f * Time.deltaTime);
		}
	}

	public void UpWeaponAnimation()
	{
		animator.SetTrigger("UpWeapon");
	}

	public void StopAnimation()
	{
		animator.SetTrigger("Stop");
		player.aimIK.solver.axis = IKAxisNormal;
		//lookPoint.localPosition = lookPointNormalPos;
		//lookPoint.localPosition = new Vector3(0, -50, -5);
		StopAllCoroutines();
		coroutine = null;
	}
}
