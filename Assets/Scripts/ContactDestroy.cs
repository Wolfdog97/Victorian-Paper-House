using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDestroy : MonoBehaviour
{
	private InspectObject pickup;
	public GameObject player;

	private void Start()
	{
		pickup = player.GetComponent<InspectObject>();
	}

	private void OnTriggerEnter(Collider col)
	{
		Debug.Log("1: ????");
		if (col.gameObject.CompareTag("Prop"))
		{
			Debug.Log(col.gameObject.name);
			//pickup.DropObject();
			Destroy(col.gameObject);
		}	
	}
}
