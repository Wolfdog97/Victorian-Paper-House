using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarScript : MonoBehaviour
{
	public float activeOffset;
	protected Vector3 activatedPosition;
	protected Vector3 deactivatedPosition;
	public bool amActivated;
	public bool amDeactivated = true;

	public GameObject pillarObject;
	
	[Range(0f,20f)] public float smoothing = 5;
	
	protected Vector3 _originalPos;
	protected Vector3 _originalRot;

	// Use this for initialization
	void Start () {
		_originalPos = transform.localPosition;
		_originalRot = transform.localEulerAngles;
		
		activatedPosition = new Vector3(transform.localPosition.x, 
			(transform.localPosition.y + activeOffset), transform.localPosition.z);
		
		deactivatedPosition = _originalPos;
		
		amDeactivated = true;
	}
	
	// Rewrite: Just call the function
	void Update () {
		if (amActivated)
		{
			PillarActivated();
			amDeactivated = false;
			StartCoroutine(WaitToEnable());
		}
		else if (amDeactivated)
		{
			PillarDeactivated();
			amActivated = false;
		}

		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			amActivated = true;
			amDeactivated = false;
		}

		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			amActivated = false;
			amDeactivated = true;	
		}
	}

	public void PillarActivated()
	{
		transform.localPosition =
			Vector3.Lerp(transform.localPosition, activatedPosition, (Time.deltaTime * smoothing));
	}

	public void PillarDeactivated()
	{
		transform.localPosition = 
			Vector3.Lerp(transform.localPosition, deactivatedPosition, (Time.deltaTime * smoothing));
	}
	
	// Need orbs to activate after the pillar has reached activated position
	public void enableObj(GameObject obj)
	{
		if (obj != null)
		{
			obj.SetActive(true);
		}
	}
	
	public void disableObj(GameObject obj)
	{
		if (obj != null)
		{
			obj.SetActive(false);
		}
	}

	IEnumerator WaitToEnable()
	{
		yield return new WaitForSecondsRealtime(2f);
		enableObj(pillarObject);
	}
	IEnumerator WaitToDisable()
	{
		yield return new WaitForSecondsRealtime(.5f);
		disableObj(pillarObject);
	}
}
