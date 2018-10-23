using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Serialization;
using UnityStandardAssets.Characters.FirstPerson;
/*
 * This Script is a mess! Rewrite needed!
 *
 * Missing or Broken:
 * Selecting tabs
 */

public class InspectObject : MonoBehaviour {

	public Camera mainCamera;
    GameObject carriedObject;

    public RigidbodyFirstPersonController rigidbodyFirstPersonController;

    public Transform holdingGuide;
    
    private Transform tempTrans;
    private Vector3 _mousePos;
    
    public bool isHolding;
    public bool holdingMode;

    private bool _inspecting;

    public float inspectDistance = 1;
    public float pickupDistance = 3;
    public float holdingDistance = 2;
    public float smoothing = 10;

    private Quaternion _inspectCameraRot;
    private Quaternion _inspectItemRot;
    
    public float rotSmoothing = 5;
    public float rotSpeed = 8;


    private Vector3 _objOriginalPos;
    private Vector3 _objOriginalRot;
    
	
	void Update () {
        if (isHolding)
        {
            Inspect(carriedObject);
            checkDrop();
        }
        else
        {
            Pickup();
        }

	    if (isHolding && holdingMode)
	    {
	        HoldItem(carriedObject); 
	    }

	    if (Input.GetKeyDown(KeyCode.P))
	    {
	        Debug.Log("In Update: " + _objOriginalPos);
	        Debug.Log("In Update: " + _objOriginalRot);
	    }
	}
    
    void Inspect(GameObject obj)
    {
        if(obj != null && !holdingMode)
        {
            // Change bool
            _inspecting = true;
            
            //Move the object into position
            obj.transform.position = Vector3.Lerp(obj.transform.position,
            mainCamera.transform.position + mainCamera.transform.forward * inspectDistance, Time.deltaTime * smoothing);
            
            // Lock Mouse Look
            rigidbodyFirstPersonController.enabled = false;
            
            // Unlock Mouse cursor
            rigidbodyFirstPersonController.mouseLook.SetCursorLock(false);
            
            //Setting the look rotation of the camera during while Inspecting 
            _inspectCameraRot *= Quaternion.Euler(0,0,0);
            
            mainCamera.transform.localEulerAngles = new Vector3(Mathf.Lerp(mainCamera.transform.localEulerAngles.x, 0, Time.deltaTime * rotSmoothing), 
                mainCamera.transform.localEulerAngles.y, mainCamera.transform.localEulerAngles.z);

            //Tell prop to Instantiate item option tags
            
            // Mode switching 
            if (Input.GetKeyDown(KeyCode.Q))
            {
                HoldingMode();
            }
            if (Input.GetMouseButton(1))
            {
                RotateItem();
            }
        }   
    }

    //(For rewrite) Action
    void Pickup()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider);

                Prop pickupable = hit.collider.GetComponent<Prop>();
                if(pickupable != null && Vector3.Distance(pickupable.gameObject.transform.position, mainCamera.transform.position) < pickupDistance)
                {
                    // Getting item original Location
                    _objOriginalPos = pickupable.originalPos;
                    _objOriginalRot = pickupable.originalRot;
                    
                    
                    isHolding = true;
                    carriedObject = pickupable.gameObject;
                    pickupable.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    
                    carriedObject.transform.parent = gameObject.transform;
                    
                    Debug.Log("After Pickup: " + _objOriginalPos);
                    Debug.Log("After Pickup: " + _objOriginalRot);
                }
            }
        }
    }
    
    // Rewrite
    void HoldItem(GameObject item)
    {
        if (!rigidbodyFirstPersonController.enabled)
        {
            rigidbodyFirstPersonController.enabled = true;
            rigidbodyFirstPersonController.mouseLook.SetCursorLock(true);
        }
        
        if (item != null)
        {
            item.transform.position = Vector3.Lerp(item.transform.position,
                holdingGuide.position + mainCamera.transform.forward * holdingDistance, Time.deltaTime * smoothing);
            
            _inspectItemRot *= Quaternion.Euler(0,0,0);
            item.transform.rotation = _inspectItemRot;
        }
    }

    void checkDrop()
    {
        if (Input.GetMouseButtonDown(0) && holdingMode)
        {
            dropObject();
        }
        
        if (Input.GetKeyDown(KeyCode.E) && _inspecting)
        {
            PutBackObj();
        }
    }

    void dropObject()
    {
        if (carriedObject != null)
        {
            isHolding = false;
            holdingMode = false;
            _inspecting = false;
            
            rigidbodyFirstPersonController.mouseLook.SetCursorLock(true);
            rigidbodyFirstPersonController.enabled = true;

            carriedObject.transform.parent = null;
            carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            
            
            carriedObject = null;
        }
    }

    void HoldingMode()
    {
        holdingMode = true;
        _inspecting = false;
    }

    void PutBackObj()
    {
        if (carriedObject != null)
        {
            isHolding = false;
            holdingMode = false;
            _inspecting = false;
            
            rigidbodyFirstPersonController.mouseLook.SetCursorLock(true);
            rigidbodyFirstPersonController.enabled = true;

            carriedObject.transform.parent = null;
            carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            
            carriedObject.transform.position = _objOriginalPos;
            carriedObject.transform.localEulerAngles = _objOriginalRot;
            
            
            carriedObject = null;
        }
    }

    // Needs Work
    void RotateItem()
    {
        // Setting Camera Rotation to Mouse Position     
        Vector3 mouseDelt = _mousePos - Input.mousePosition;
        carriedObject.transform.Rotate(new Vector3(mouseDelt.y, mouseDelt.x, 0));
       
        // Get Mouse position
        _mousePos = Input.mousePosition;
    }
}
