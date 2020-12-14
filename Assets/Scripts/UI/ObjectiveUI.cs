using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
	[SerializeField] private Text objectiveText;

	private void OnEnable()
	{
		PlayerObjective.ObjectiveChangeEvent += SetObjectiveOnUI;
	}

	private void OnDisable()
	{
		PlayerObjective.ObjectiveChangeEvent -= SetObjectiveOnUI;
	}

	private void SetObjectiveOnUI(string objective)
	{
		objectiveText.text = objective;
	}
}
