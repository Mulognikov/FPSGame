using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public Animator animator;
	public PlayerHealth playerHealth;
	public PlayerAnim playerAnim;
	public AimIK aimIK;
	public PlayerWeaponVer2 playerWeapon;
	public PlayerMove playerMove;


	[HideInInspector] public int BombCount;

	private void Start()
	{
		PlayerList.PL.PlayersList.Clear();
		PlayerList.PL.PlayersList.Add(this);
	}
}
