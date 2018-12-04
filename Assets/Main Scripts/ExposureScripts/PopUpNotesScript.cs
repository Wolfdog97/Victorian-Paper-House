using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Issues:
 * Rotating around the x axis
 * 
 */
public class PopUpNotesScript : MonoBehaviour
{

	private GameObject _target;
	[Range(1,10)]public int damping = 5;

	public bool amLooking = true;

	void Start()
	{
		_target = GameObject.FindWithTag("Player");
	}
	
	void Update () 
	{
		//transform.LookAt(_target.transform);
		if (amLooking)
		{
			LookAtPlayer();
		}
	}

	void LookAtPlayer()
	{
		var lookPos = _target.transform.position - transform.position;
		lookPos.y = 0;
		Quaternion rotation = Quaternion.LookRotation(lookPos);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
	}

	private void OnEnable()
	{
		//Play Sound
		Debug.Log(name + "Enabled!");
	}

	private void OnDisable()
	{
		//Play Sound
		Debug.Log(name + "Disabled!");
	}
}
