using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class UIWorldDetect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		RaycastWorldUI();
	}

	void RaycastWorldUI()
	{
		if (Input.GetMouseButtonDown(0))
		{
			
			PointerEventData pointerData = new PointerEventData(EventSystem.current);

			pointerData.position = Input.mousePosition;
			
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerData, results);
	
			Debug.Log(results.Count);
			if (results.Count > 0)
			{
				if (results[0].gameObject.layer == LayerMask.NameToLayer("WorldUI"))
				{
					string dbg = "Root Element: {0} /n GrandChild Element: {1}";
					Debug.Log(string.Format(dbg, results[results.Count-1].gameObject.name,results[0].gameObject.name));
					
					results.Clear();
				}
			}
		}
	}
}
