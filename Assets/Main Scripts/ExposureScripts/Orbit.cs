using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{

	public float theta = 0.1f;
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(Vector3.zero, Vector3.right, theta * Time.deltaTime);
	}
}
