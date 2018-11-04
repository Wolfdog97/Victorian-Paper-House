using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : Pickupable
{
	[Header("Read Only: ")]
	[SerializeField] protected Vector3 _originalPos;
	[SerializeField] protected Vector3 _originalRot;
	[HideInInspector] public bool amPickedUp;
	[Space(2)]
	
	public bool printOriginalLocation = false;

	// Private vars returned by public function so the positions can be read, but cannot be changed
	public Vector3 originalPos
	{
		get { return _originalPos; }
	}
	public Vector3 originalRot
	{
		get { return _originalPos; }
	}

	private void Awake()
	{
		_originalPos = transform.position;
		_originalRot = transform.localEulerAngles;
	}

	void Update()
	{
		if (printOriginalLocation)
		{
			Debug.Log(name + "OG Pos: " + _originalPos);
			Debug.Log(name + "OG Rot: " + _originalRot);
		}
		if (amPickedUp)
		{
			Physics.IgnoreLayerCollision(10,11);
		}
	}
	
}
