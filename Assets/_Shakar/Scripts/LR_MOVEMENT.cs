using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LR_MOVEMENT : MonoBehaviour
{

	private bool _dirRight = true;
	public float _speed = 0.1f;
	public float _yMax; 
	public float _yMin; 

	
	void Update ()
	{

		if (_dirRight)
			transform.Translate(Vector3.up * _speed * Time.deltaTime);
		else
		{
			transform.Translate(-Vector3.up * _speed * Time.deltaTime);
			
		}

		if (transform.position.y >= _yMax)
		{
			_dirRight = false; 
		}

		if (transform.position.y <= _yMin)
		{
			_dirRight = true; 
		}
	}
}
