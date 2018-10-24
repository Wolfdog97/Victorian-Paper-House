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

public class InspectObject : MonoBehaviour
{

    [Header("Keys: ")]
    [Space(2)]
    public KeyCode holdItemKey = KeyCode.Alpha1;
    public KeyCode putBackKey = KeyCode.Alpha2;
    public KeyCode swapItemKey = KeyCode.Alpha3;
    [Space(10)]
    
	public Camera mainCamera;
    Prop carriedObject;

    public RigidbodyFirstPersonController rigidbodyFirstPersonController;

    public Transform holdingGuide;
    [Space(10)]
    
    
    // Bools
    [HideInInspector]
    public bool isHolding;
    [HideInInspector]
    public bool holdingMode;
    
    private bool _isInspecting = false;
    private bool _isPlaceToPutBack = false;

    
    [Header("Distance & Smoothing: ")]
    [Space(2)]
    [Range(0f,3f)] public float holdingDistance = 2;
    [Range(1f,20f)] public float smoothing = 10;
    [Range(1f,20f)] public float rotSmoothing = 5;

    float inspectDistance = 1;
    float pickupDistance = 3;
    
    // Private Vars
    private Transform tempTrans;
    private Quaternion _inspectCameraRot;
    private Quaternion _inspectItemRot;
    private Vector3 _objOriginalPos;
    private Vector3 _objOriginalRot;
    private Vector3 _mousePos;
    
    // Strings
    [Header("GUI Text: ")] 
    [Space(2)] 
    public string putBackText = "Put Back";
    public string placeHereText = "Place";
    [Space(10)]
    
    // TO BE CHANGED!
    [Header("Temp: ")]
    [Space(2)]
    public Sprite sprtToSwap;
    public SpriteRenderer spriteRenderer;
    
	
	void Update () {
        if (isHolding)
        {
            Inspect(carriedObject);
            CheckDrop();
        }
        else
        {
            PickupObject();
        }

	    if (isHolding && holdingMode)
	    {
	        HoldItem(carriedObject); 
	    }
	}
 
    //(REWRITE!) Picking up the item and entering "Inspection Mode"
    public void PickupObject()
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
                
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.green); // Drawing ray
                if(pickupable != null && 
                   Vector3.Distance(pickupable.transform.position, mainCamera.transform.position) < pickupDistance)
                {
                    // Getting item original Location
                    _objOriginalPos = pickupable.originalPos;
                    _objOriginalRot = pickupable.originalRot;
                    
                    
                    isHolding = true;
                    carriedObject = pickupable;
                    carriedObject.transform.parent = gameObject.transform;
                    pickupable.GetComponent<Rigidbody>().isKinematic = true;
                    pickupable.amPickedUp = true;


                    //Debug.Log("After PickupObject: " + _objOriginalPos);
                    //Debug.Log("After PickupObject: " + _objOriginalRot);
                }
            }
        }
    }
    
    public void TextOnRaycast()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x,y));
        RaycastHit _hit;

        if (Physics.Raycast(ray, out _hit, pickupDistance))
        {
            _isPlaceToPutBack = true;
        }
    }

    void OnGui()
    {
        if (_isPlaceToPutBack)
        {
            GUILayout.Label(putBackText);// Text for placing objects
        }
    }
    
    public void Inspect(Prop obj)
    {
        if(obj != null && !holdingMode)
        {
            // Change bool
            _isInspecting = true;
            
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
    public void HoldItem(Prop item)
    {
        // Unlock cursor and FP Character controller
        if (!rigidbodyFirstPersonController.enabled)
        {
            rigidbodyFirstPersonController.enabled = true;
            rigidbodyFirstPersonController.mouseLook.SetCursorLock(true);
        }
        
        // Changing Item Position
        if (item != null)
        {
            item.transform.position = Vector3.Lerp(item.transform.position,
                holdingGuide.position + mainCamera.transform.forward * holdingDistance, Time.deltaTime * smoothing);
            
            _inspectItemRot *= Quaternion.Euler(0,0,0);
            item.transform.rotation = _inspectItemRot;
        }
    }

    void CheckDrop()
    {
        if (Input.GetMouseButtonDown(0) && holdingMode)
        {
            DropObject();
        }
        
        if (Input.GetKeyDown(putBackKey) && _isInspecting)
        {
            PutBackObj();
        }
    }

    public void DropObject()
    {
        if (carriedObject != null)
        {
            isHolding = false;
            holdingMode = false;
            _isInspecting = false;
            
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
        _isInspecting = false;
    }

    public void PutBackObj()
    {
        if (carriedObject != null)
        {
            isHolding = false;
            holdingMode = false;
            _isInspecting = false;
            
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
    private void RotateItem()
    {
        // Setting Camera Rotation to Mouse Position     
        Vector3 mouseDelt = _mousePos - Input.mousePosition;
        carriedObject.transform.Rotate(new Vector3(mouseDelt.y, mouseDelt.x, 0));
       
        // Get Mouse position
        _mousePos = Input.mousePosition;
    }

    public void SwapObject()
    {
        spriteRenderer.sprite = sprtToSwap;
    }
}
