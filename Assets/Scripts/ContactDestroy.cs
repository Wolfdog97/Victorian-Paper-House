using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDestroy : MonoBehaviour
{
	private PickupObject pickup;
	public GameObject player;

	private void Start()
	{
		pickup = player.GetComponent<PickupObject>();
	}

	private void OnTriggerEnter(Collider col)
	{
		Debug.Log("1: ????");
		if (col.gameObject.CompareTag("Prop"))
		{
			Debug.Log(col.gameObject.name);
			Destroy(col.gameObject);

			pickup.carrying = false;
		}
			
	}
}
