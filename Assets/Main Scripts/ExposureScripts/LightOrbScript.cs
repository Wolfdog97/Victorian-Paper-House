using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOrbScript : Prop
{
	[Header("LightOrb")]
	public bool lightIsOn;

	public List<GameObject> lightsIControl;

	private void Update()
	{
		LightSwitch();
		if (lightIsOn)
		{
			TurnOnAllLights();
		}
	}

	public void LightSwitch()
	{
		if (objPlaced)
		{
			lightIsOn = true;
		}
	}

	public void TurnOnAllLights()
	{
		foreach (var light in lightsIControl)
		{
			light.SetActive(true);
			break;
		}
	}

}
