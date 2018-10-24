using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : Pickupable
{
	
	[HideInInspector] public Vector3 originalPos;
	[HideInInspector] public Vector3 originalRot;
	[HideInInspector] public bool amPickedUp;
	public bool printLoc = false;

	private void Awake()
	{
		originalPos = transform.position;
		originalRot = transform.localEulerAngles;
	}

	void Update()
	{
		if (printLoc)
		{
			Debug.Log(name + "OG Pos: " + originalPos);
			Debug.Log(name + "OG Rot: " + originalRot);
		}

		if (amPickedUp)
		{
			Physics.IgnoreLayerCollision(10,11);
		}
	}
	
}
