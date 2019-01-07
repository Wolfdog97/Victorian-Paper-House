using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxCamera : MonoBehaviour
{
	private Camera _camera;
	
	// Use this for initialization
	void Start ()
	{
		_camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		_camera.fieldOfView = Camera.main.fieldOfView;
		transform.rotation = Camera.main.transform.rotation;
	}
}
