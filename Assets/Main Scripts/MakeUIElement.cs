using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class MakeUIElement : MonoBehaviour, IPointerClickHandler
{

	[SerializeField] private GameObject uiToInstantiate;
	public Vector3 offset = new Vector3(0,0,0);
	public bool elementActive;

	public GameObject uiToEnable;
	
	// not used
	public void OnPointerClick(PointerEventData eventData)
	{
		Vector3 ScreenPosition = new Vector3(eventData.position.x, eventData.position.y, 
			eventData.pointerPressRaycast.distance);
		Vector3 instantiatePosition = eventData.pressEventCamera.ScreenToWorldPoint(ScreenPosition);
		GameObject clone = (GameObject) Instantiate(uiToInstantiate, instantiatePosition,
			eventData.pressEventCamera.transform.rotation);
		clone.transform.SetParent(transform);
	}

	public void CreateUIElement()
	{
		// need to be able to disable/destroy the clone. or make it enable/disable instead
		if (!elementActive)
		{
			Debug.Log("Creating Element..");
			Vector3 instantiatePosition = new Vector3(transform.localPosition.x + offset.x, 
				(transform.localPosition.y + offset.y), transform.localPosition.z + offset.z);
			//later the UI element will always face the player
			GameObject _clone = (GameObject) Instantiate(uiToInstantiate, instantiatePosition, Quaternion.identity);
			_clone.transform.SetParent(transform);
			elementActive = true;
		}
	}

	public void EnableUI()
	{
		uiToEnable.SetActive(true);
		elementActive = true;
	}
	public void DisableUI()
	{
		uiToEnable.SetActive(false);
		elementActive = false;
	}
}
