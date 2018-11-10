using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapableObject : MonoBehaviour
{

	public SpriteRenderer sRenderer;
	public Sprite newSprite;

	public bool haveSwapped;
	
	void Update () {
		
	}

	public void SwapObjectSprite()
	{
		if (!haveSwapped)
		{
			sRenderer.sprite = newSprite;

			haveSwapped = true;
		}
	}
}
