using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementLocation : MonoBehaviour
{

	public GameObject targetProp;
	public PuzzleManager pManager;
	public bool weGood;

	public List<GameObject> targetObjects;
	
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CheckCondition(GameObject propObject)
	{
		if (propObject == targetProp)
		{
			pManager.LocConditionMet();
		}
	}

	// incomplete 
	void CheckListCondition()
	{
		foreach (var obj in targetObjects)
		{
			CheckCondition(obj);
			break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		CheckCondition(other.gameObject);
	}
}
