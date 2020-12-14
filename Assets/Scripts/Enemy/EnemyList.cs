using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
	public static EnemyList EL;

	[HideInInspector] public List<Enemy> Enemies = new List<Enemy>();
	[HideInInspector] public List<Turret> Turrets = new List<Turret>();

	private void Awake()
	{
		EL = this;
	}

	public void AddEnemy(Enemy enemy)
	{
		Enemies.Add(enemy);
	}

	public void AddTurret(Turret turret)
	{
		Turrets.Add(turret);
	}
}
