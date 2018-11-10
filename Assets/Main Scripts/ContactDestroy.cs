using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDestroy : MonoBehaviour
{
	// Broken
	private void OnTriggerEnter(Collider col)
	{
		Prop _prop = col.gameObject.GetComponent<Prop>();
		
		Debug.Log("1: ????");
		if (_prop != null)
		{
			Debug.Log(_prop.name);
			Destroy(_prop.gameObject);
		}	
	}
}
