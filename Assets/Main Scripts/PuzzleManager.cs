using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{

	public static PuzzleManager me;

	public bool locCondition;
	public List<PlacementLocation> pLocs; 	
	public List<PuzzleConditional> conditions;
	
	

	private void Awake()
	{
		if (me == null)
		{
			me = this;
		}
		else if (me != this)
		{
			Destroy(gameObject);
			return;
		}
		
		DontDestroyOnLoad(gameObject);
	}

	public void LocConditionMet()
	{
		locCondition = true;
	}

	bool AllConditionsMet()
	{
		for (int i = 0; i < pLocs.Count; i++)
		{
			if (!pLocs[i].weGood)
				return false;
		}
		for (int i = 0; i < conditions.Count; i++)
		{
			if (!conditions[i].conditionMet)
				return false;
		}
		return true;
	}
}
