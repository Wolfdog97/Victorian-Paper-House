using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarButton : MonoBehaviour
{

	public List<PillarScript> pillarsToActivate;

	public bool amPressed;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (amPressed)
		{
			ActivatePillars();
		}
	}

	public void ActivatePillars()
	{
		if (pillarsToActivate.Count != 0)
		{
			for (int i = 0; i < pillarsToActivate.Count; i++)
			{
				// Enable all of the tags
				pillarsToActivate[i].amActivated = true;
			}
		}
	}
}
