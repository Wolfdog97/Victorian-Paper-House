using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * To Add:
 * The ability to raise and lower a pillar
 * Anim when pressed
 * Sound pressed
 * (on Pillars) Sound when moving
 * Indication that button was pressed.
 *
 * Add the ability to change "Type"(button or trigger) in the inspector
 */
public class PillarButton : MonoBehaviour
{

	public List<PillarScript> pillarsToActivate;

	public bool amPressed;
	public bool amTriggered;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (amPressed || amTriggered)
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
	
	//For the trigger
	private void OnTriggerEnter(Collider other)
	{
		amTriggered = true;
	}
}
