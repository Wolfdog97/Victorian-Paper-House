using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
/*
 * Notes:
 * But what if it is placed in a new location and put back is called? inNewLoc variable?
 */


public class Prop : Pickupable
{
	public string displayName = "default";
	public InspectObject player;
	
	[Header("Prop Tags: ")]
	public GameObject[] tags;
	[Space(2)] 
	
	[Header("Where I can be placed")]
	public List<PlacementLocation> validLocations;

	[Header("Read Only: ")] 
	[SerializeField] protected Vector3 _originalPos;
	[SerializeField] protected Vector3 _originalRot;
	[HideInInspector] public bool amPickedUp;
	[Space(2)]
	
	[Header("Debug: ")] 
	public bool tagsAreActive;
	public bool printOriginalLocation;
	public bool printCurrentLocation;
	public bool objPlaced;
	
	// Causes Rotation issue
	public Vector3 propInspectOffset = new Vector3(0,0,0);
	
	
	// Private vars returned by public function so the positions can be read, but cannot be changed
	public Vector3 originalPos
	{
		get { return _originalPos; }
	}
	public Vector3 originalRot
	{
		get { return _originalRot; }
	}
	
	//Set OG position and rotation
	protected virtual void Start()
	{
		_originalPos = transform.position;
		_originalRot = transform.eulerAngles;
	}

	protected virtual void Update()
	{
		PrintLocations();
		ShowTags();
		// temp
		if (amPickedUp)
		{
			Physics.IgnoreLayerCollision(10,11);
		}
	}

	public void ShowTags()
	{
		// needs to be changed so that it only happens when a single object is picked up
		if (player != null)
		{
			if (player.inspectionMode || tagsAreActive) //if the player is Inspecting
			{
				for (int i = 0; i < tags.Length; i++)
				{
					// Enable all of the tags
					tags[i].SetActive(true);
					
					Debug.Log(name + " is showing tabs");
				}
			}
			else
			{
				for (int i = 0; i < tags.Length; i++)
				{
					tags[i].SetActive(false);
				}
			}
		}
	}

	public void PrintLocations()
	{
		if (printOriginalLocation)
		{
			Debug.Log(name + "OG Pos: " + _originalPos);
			Debug.Log(name + "OG Rot: " + _originalRot);
		}
		
		if (printCurrentLocation)
		{
			Debug.Log(name + "Current Pos: " + transform.localPosition);
			Debug.Log(name + "Current Rot: " + transform.localEulerAngles);
		}
	}
	
	public bool CheckLocation(PlacementLocation loc, List<PlacementLocation> list)
	{
		// Iterate through list
		// Check if that transform is not in the list
		// If so, return true
		if (loc == null || list.Count.Equals(0) ) return false;
		
		foreach (var t in list)
		{
			if (!list.Contains(loc)) return false;
		}
		
		return true;
	}
	
}
