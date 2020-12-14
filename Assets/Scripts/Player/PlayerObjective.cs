using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjective : MonoBehaviour
{
	public static PlayerObjective PO;

	public delegate void ObjectiveChangeEventHandler(string objective);
	public static event ObjectiveChangeEventHandler ObjectiveChangeEvent;

	private Objective findBomb = new Objective
	{
		objective = "Найти заряды cо взрывчаткой",
		target = 3,
		isActive = true,
	};

	private Objective findBombPlant = new Objective
	{
		objective = "Найти место установки зарядов",
		target = 1,
		isActive = true
	};

	private Objective plantBomb = new Objective
	{
		objective = "Установить заряды с взрывчаткой",
		target = 3,
		isActive = false
	};

	private Objective leave = new Objective
	{
		objective = "Вернуться на свой корабль",
		target = 1,
		isActive = false
	};

	private Objective[] objectives;
	private Coroutine routine;

	private void Awake()
	{
		PO = this;
		objectives = new Objective[] { findBomb, findBombPlant, plantBomb, leave };
	}

	private void Start()
	{
		ObjectiveChangeEvent?.Invoke(MergeAllObjectiveToString());
	}

	private string MergeAllObjectiveToString()
	{
		string allObjecives = "";

		foreach(Objective o in objectives)
		{
			if (o.isHidden)
			{
				continue;
			}

			if (o.isActive)
			{
				if (o.isDone)
				{
					if (o.target > 1)
						allObjecives += ("<color=lime>" + "\u2713" + o.objective + " (" + o.progress + "/" + o.target + ") " + "</color>" + "\n");
					else
						allObjecives += ("<color=lime>" + "\u2713" + o.objective + "</color>" + "\n");
				}
				else
				{
					if (o.target > 1)
						allObjecives += ("\u2022 " + o.objective + " (" + o.progress + "/" + o.target + ") " + "\n");
					else
						allObjecives += ("\u2022 " + o.objective + "\n");
				}
			}
		}

		return allObjecives;
	}

	public void BombFinded()
	{
		findBomb.progress += 1;
		ObjectiveChangeEvent?.Invoke(MergeAllObjectiveToString());

		if (findBomb.progress == findBomb.target)
		{
			if (routine == null)
			{
				routine = StartCoroutine(ObjectiveDone(findBomb, 3));
			}
			else
			{
				routine = StartCoroutine(ObjectiveDone(findBomb, 5));
			}

		}
	}

	public void PlantFinded()
	{
		findBombPlant.progress = 1;
		ObjectiveChangeEvent?.Invoke(MergeAllObjectiveToString());

		if (routine == null)
		{
			routine = StartCoroutine(ObjectiveDone(findBombPlant, 3, plantBomb));
		}
		else
		{
			routine = StartCoroutine(ObjectiveDone(findBombPlant, 5, plantBomb));
		}

	}

	public void BombPlanted(int bombCount)
	{
		plantBomb.progress = bombCount;
		ObjectiveChangeEvent?.Invoke(MergeAllObjectiveToString());

		if (plantBomb.progress == plantBomb.target)
		{
			if (routine == null)
			{
				routine = StartCoroutine(ObjectiveDone(plantBomb, 3, leave));
			}
			else
			{
				routine = StartCoroutine(ObjectiveDone(plantBomb, 5, leave));
			}

		}
	}

	public void Leaved()
	{
		leave.progress = 1;
		ObjectiveChangeEvent?.Invoke(MergeAllObjectiveToString());
	}

	IEnumerator ObjectiveDone(Objective doneObjective, int time , Objective nextObjective = null)
	{
		doneObjective.isDone = true;
		ObjectiveChangeEvent?.Invoke(MergeAllObjectiveToString());

		if (nextObjective != null)
		{
			nextObjective.isActive = true;
			nextObjective.isHidden = true;
		}

		yield return new WaitForSeconds(time);

		doneObjective.isActive = false;

		if (nextObjective != null)
		{
			nextObjective.isHidden = false;
		}

		routine = null;
		ObjectiveChangeEvent?.Invoke(MergeAllObjectiveToString());
	}
}

class Objective
{
	public string objective;
	public int progress = 0;
	public int target;
	public bool isActive;
	public bool isDone;
	public bool isHidden;
}
