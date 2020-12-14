using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
	public GameObject enemy;
	public int SpawnChance;

	private bool spawn = false;
	private static int objectsCount = 0;
	private static int spawnObjectsCount = 0;

	protected void FirstSpawn()
	{
		int rnd = Random.Range(0, 100);

		if (rnd < SpawnChance)
		{
			spawn = true;
			spawnObjectsCount++;
		}

		objectsCount++;
	}

	protected void TrySpawn()
	{
		float spawnedChance = spawnObjectsCount / objectsCount * 100;
		float delta = Mathf.Abs(spawnedChance - SpawnChance);

		if (delta < 5 && spawnObjectsCount > 1)
		{
			return;
		}

		if (spawnedChance < SpawnChance)
		{
			if (!spawn)
			{
				int rnd = Random.Range(0, 100);

				if (rnd < SpawnChance)
				{
					spawn = true;
					spawnObjectsCount++;
				}
			}
		}
		else
		{
			if (spawn)
			{
				int rnd = Random.Range(0, 100);

				if (rnd < SpawnChance)
				{
					spawn = false;
					spawnObjectsCount--;
				}
			}
		}
	}

	protected void Spawn()
	{
		if (spawn)
		{
			Instantiate(enemy, transform.position, transform.rotation, transform);
		}

		enabled = false;
	}

	private void Awake()
	{
		FirstSpawn();
	}

	private void Start()
	{
		TrySpawn();
	}

	private void Update()
	{
		TrySpawn();
	}

	private void LateUpdate()
	{
		Spawn();
	}
}
