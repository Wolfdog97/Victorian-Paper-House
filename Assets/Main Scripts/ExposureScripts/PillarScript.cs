using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarScript : MonoBehaviour
{
	public float activeOffset;
	protected Vector3 activatedPosition;
	public bool amActivated;
	
	[Range(1f,20f)] public float smoothing = 5;
	
	// Use this for initialization
	void Start () {
		activatedPosition = new Vector3(transform.localPosition.x, 
			(transform.localPosition.y + activeOffset), transform.localPosition.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (amActivated)
		{
			PillarActivated();
			Debug.Log(transform.localPosition);
		}
	}

	public void PillarActivated()
	{
		
		
		
		transform.localPosition =
			Vector3.Lerp(transform.localPosition, activatedPosition, (Time.deltaTime * smoothing));
	}
}
