using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour { // **SCRIPT TO MAKE AN OBJECT ROTATE AROUND ANOTHER ONE**

	[Range(0f, 2f * Mathf.PI)] 			// make a slider with a range of 0 to 2Pi 
	public float angle; 				// angle value

	public float randomrange;			// custom range for random values
	public float radius; 				// radius value


	void Start () {								// Start function
		randomrange = Random.Range (1f, 1.05f);	// setting the range for the random number
	}// END START


	void Update () {																							// Update function
		Vector3 newpos = PointOnCircle (radius + randomrange, Mathf.Rad2Deg * Time.time + Time.time);			//***???***
		this.transform.position = newpos;																		//***???***
	}// END UPDATE


	public Vector3 PointOnCircle(float radius, float angle) {													//***???***
		float angleInRadians = angle * Mathf.Deg2Rad; 															// mathf.deg2rad is a constant value of 180/pi, (stands for degrees 2 radians)
		return new Vector3 (radius * Mathf.Cos (angleInRadians),			// x value
//		return new Vector3 (GameManager.instance.amountZoomedOut* radius * Mathf.Cos (angleInRadians),			// x value
			radius * Mathf.Sin (angleInRadians), 																// y value
			0f); 																								// z value
	}//end PointOnCircle 


} //END SCRIPT




// ~~~ making the ai enemies spawn in a circle around the player:
// ~~~ float enemySpawnAngle = Random.Range (-Mathf.PI, Mathf.PI);
