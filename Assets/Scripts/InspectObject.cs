using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
/*
 * This Script is a mess! Rewrite needed!
 *
 * Missing or Broken:
 * Selecting tabs
 * Display text or change cursor 
 */

public class InspectObject : MonoBehaviour
{
    // Public Vars
    public Camera mainCamera;
    public RigidbodyFirstPersonController rigidbodyFirstPersonController;
    public Transform holdingGuide;
    [Space(10)]
    
    [Header("Input: ")]
    [Space(2)]
    public KeyCode holdItemKey = KeyCode.Alpha1;
    public KeyCode putBackKey = KeyCode.Alpha2;
    public KeyCode swapItemKey = KeyCode.Alpha3;
    public KeyCode interactionKey = KeyCode.Alpha4;
    [Space(10)]
    
	
    [Header("Modes: ")]
    [Space(2)]
    public bool inspectionMode;
    public bool holdingMode;
    [Space(10)]
    
    // Bools
    [HideInInspector]
    public bool isCarrying;
    
    //private bool _isPlaceToPutBack = false;

    [Header("Distance & Smoothing: ")]
    [Space(2)]
    [Range(0f,3f)] public float holdingDistance = 2;
    [Range(1f,20f)] public float smoothing = 10;
    [Range(1f,20f)] public float rotSmoothing = 5;
    protected float inspectDistance = 1;
    protected float pickupDistance = 3;
    [Space(10)]
    
    // Private Vars
    protected Transform tempTrans;
    protected Quaternion _inspectCameraRot;
    protected Quaternion _inspectItemRot;
    protected Vector3 _objOriginalPos;
    protected Vector3 _objOriginalRot;
    protected Vector3 _mousePos;
    protected Prop carriedObject;
    
    
    // Strings
    [Header("GUI Text: ")] 
    [Space(2)] 
    public string putBackText;
    public string placeHereText;
    [Space(10)]
    
    // TO BE CHANGED!
    [Header("Temp: ")]
    [Space(2)]
    public Sprite sprtToSwap;
    public SpriteRenderer spriteRenderer;
    public GameObject rugtoMove;
    public Transform newRugLoc;
    
	
	void Update () {
	    /*
	     * To Clarify: Change to Mode/State Switching.
	     * Modes: Inspection, Holding
	     */
	    
        if (isCarrying)
        {
            Inspect(carriedObject);
            
            CheckDrop();
            
            if (holdingMode)
            {
                HoldItem(carriedObject);
            }
        }
        else
        {
            PickupObject();
        }

	    if (Input.GetKeyDown(interactionKey))
	    {
	        moveObject();
	    }
	    
	    //TextOnRaycast();
	}
 
    // (REWRITE!) Picking up the item and entering "Inspection Mode"
    // Note: Seperate shooting Raycast from PickupObject()?
    // This function should only cover the picking up option
    public void PickupObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit = new RaycastHit();
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider);

                Prop pickupable = hit.collider.GetComponent<Prop>();
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.green); // Drawing ray
                
                if(pickupable != null && 
                   Vector3.Distance(pickupable.transform.position, mainCamera.transform.position) < pickupDistance)
                {
                    // Getting item original Location
                    _objOriginalPos = pickupable.originalPos;
                    _objOriginalRot = pickupable.originalRot;
                    
                    isCarrying = true;
                    carriedObject = pickupable;
                    carriedObject.transform.parent = gameObject.transform;
                    pickupable.GetComponent<Rigidbody>().isKinematic = true;
                    pickupable.amPickedUp = true;
                }
                
                // Temp
                if (hit.Equals(gameObject.CompareTag("Target")))
                {
                    moveObject();
                }
            }
        }
    }
    
    public void Inspect(Prop obj) // Filled with carriedObject in update
    {
        if(obj != null && !holdingMode)
        {
            // Change bool
            inspectionMode = true;
            
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
            
            // Mode switching (Move to update?)
            if (Input.GetKeyDown(holdItemKey))
            {
                HoldingMode();
            }
            if (Input.GetMouseButton(1))
            {
                RotateItem();
            }
            if (Input.GetKeyDown(swapItemKey))
            {
                SwapObject();
            }
        }   
    }

    // Rewrite
    public void HoldItem(Prop obj)
    {
        // Unlock cursor and FP Character controller
        if (!rigidbodyFirstPersonController.enabled)
        {
            rigidbodyFirstPersonController.enabled = true;
            rigidbodyFirstPersonController.mouseLook.SetCursorLock(true);
        }
        
        // Changing Item Position
        if (obj != null)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position,
                holdingGuide.position + mainCamera.transform.forward * holdingDistance, Time.deltaTime * smoothing);
            
            _inspectItemRot *= Quaternion.Euler(0,0,0);
            obj.transform.rotation = _inspectItemRot;
        }
    }

    void CheckDrop()
    {
        if (Input.GetMouseButtonDown(0) && holdingMode)
        {
            DropObject();
        }
        
        if (Input.GetKeyDown(putBackKey) && inspectionMode)
        {
            PutBackObj();
        }

        if (carriedObject == null)
        {
            isCarrying = false;
            holdingMode = false;
            inspectionMode = false;
        }
    }

    public void DropObject()
    {
        if (carriedObject != null)
        {
            isCarrying = false;
            holdingMode = false;
            inspectionMode = false;
            
            rigidbodyFirstPersonController.mouseLook.SetCursorLock(true);
            rigidbodyFirstPersonController.enabled = true;

            carriedObject.transform.parent = null;
            carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            
            
            carriedObject = null;
        }
    }

    public void HoldingMode()
    {
        holdingMode = true;
        inspectionMode = false;
    }

    public void PutBackObj()
    {
        if (carriedObject != null)
        {
            isCarrying = false;
            holdingMode = false;
            inspectionMode = false;
            
            rigidbodyFirstPersonController.mouseLook.SetCursorLock(true);
            rigidbodyFirstPersonController.enabled = true;

            carriedObject.transform.parent = null;
            carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            
            carriedObject.transform.position = _objOriginalPos;
            carriedObject.transform.localEulerAngles = _objOriginalRot;
            
            
            carriedObject = null;
        }
    }

    // Temp/Needs Work/Should be removed
    private void RotateItem()
    {
        // Setting Camera Rotation to Mouse Position     
        Vector3 mouseDelt = _mousePos - Input.mousePosition;
        carriedObject.transform.Rotate(new Vector3(mouseDelt.y, mouseDelt.x, 0));
       
        // Get Mouse position
        _mousePos = Input.mousePosition;
    }
    
    // Temp
    public void SwapObject()
    {
        spriteRenderer.sprite = sprtToSwap;
    }

    public void moveObject()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider);
                
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.green); // Drawing ray
            // Temp
            GameObject obj = hit.collider.gameObject;
            if (obj.tag.Equals("Target"))
            {
                rugtoMove.transform.position = newRugLoc.position;
            }
        }
    }
    
    // Not implemented
    public void TextOnRaycast()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x,y));
        RaycastHit _hit;

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow); // Drawing ray
        if (Physics.Raycast(ray, out _hit, pickupDistance))
        {
            
        }
    }
}
