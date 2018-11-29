using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotator : MonoBehaviour
{

	public GameObject sun;
	public Vector3 rotatedPos;
	public int smoothing;

	public bool amActivated;

	private Vector3 _sunRot;
	
	void Start ()
	{
		_sunRot = sun.transform.localEulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RotateSun()
	{
		sun.transform.localEulerAngles = Vector3.Slerp(sun.transform.localEulerAngles, -sun.transform.localEulerAngles, Time.deltaTime * smoothing);
	}

	public void ReturnSun()
	{
		sun.transform.localEulerAngles = Vector3.Slerp(sun.transform.localEulerAngles, _sunRot, Time.deltaTime * smoothing);
	}
	
}
