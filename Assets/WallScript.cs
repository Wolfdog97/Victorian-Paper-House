using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{

	public MakeUIElement obj;

	private void Update()
	{
		if (obj.elementActive)
		{
			gameObject.SetActive(false);
		}
	}
}
