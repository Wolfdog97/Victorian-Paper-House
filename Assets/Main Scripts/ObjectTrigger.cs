using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{

	public GameObject objectToTrigger;


	private void OnTriggerEnter(Collider other)
	{
		objectToTrigger.SetActive(false);
	}
}
