using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * Not currently working, but almost there
 */
public class ReticleController : MonoBehaviour {
	
	public Camera mainCamera;
	 
	public int rayDistance = 4;
	public GameObject reticle;
	public TextMeshProUGUI tMPGUI;

	protected InspectObject _inpsectScript;


	private void Start()
	{
		_inpsectScript = gameObject.GetComponent<InspectObject>();
	}

	private void Update()
	{
		ReticleGaze();
		//Debug.Log(_inpsectScript.inspectionMode);
	}

	// Note: This should not run during inspect mode
	public void ReticleGaze()
	{
		if (!_inpsectScript.inspectionMode)
		{
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Ray ray = mainCamera.ScreenPointToRay(new Vector3(x,y));
			RaycastHit _hit;

			if (Physics.Raycast(ray, out _hit, rayDistance))
			{
				Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow); // Drawing ray visual
				// if the ray hits an object with a tag, change the text
				
				Prop _prop = _hit.collider.GetComponent<Prop>();
				MakeUIElement noteMaker = _hit.collider.GetComponent<MakeUIElement>();
				
				if (_prop != null)
				{
					tMPGUI.text = _prop.displayName;
					reticle.SetActive(false);
				}
				else if(noteMaker != null)
				{
					tMPGUI.text = "?";
					reticle.SetActive(false);
				}
				
				//Switch for other objects 
		
			}
			else
			{
				tMPGUI.text = "";
				reticle.SetActive(true);
			}
		}
	}
}
