using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;
/*
 * This Script is a mess! Rewrite needed!
 * Notes: I only need to shoot one ray then check for conditions
 *     Should I only include Prop tag functions and related functions?
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
    public KeyCode pickupKey = KeyCode.E;
    public KeyCode holdItemKey = KeyCode.Alpha1;
    public KeyCode putBackKey = KeyCode.Alpha2;
    public KeyCode swapItemKey = KeyCode.Alpha3;
    public KeyCode interactionKey = KeyCode.Alpha4;
    public KeyCode dropObjectKey = KeyCode.Alpha5;
    [Space(10)]
    
	
    [Header("Modes: ")]
    [Space(2)]
    public bool inspectionMode;
    public bool interactingWithCanvas;
    public bool holdingMode;
    [Space(10)]
    
    // Bools
    [SerializeField]
    protected bool isCarrying;

    [Header("Distance & Smoothing: ")]
    [Space(2)]
    [Range(0f,3f)] public float holdingDistance = 2;
    [Range(1f,20f)] public float smoothing = 10;
    [Range(1f,20f)] public float rotSmoothing = 5;
    [Range(0,1f)]public float inspectDistance = 1; // Should this depend on the object?
    protected float pickupDistance = 3;
    [Space(10)]
    
    // Private Vars
    protected Transform tempTrans;
    protected Quaternion _inspectCameraRot;
    protected Quaternion _inspectItemRot;
    protected Vector3 _objOriginalPos;
    protected Vector3 _objOriginalRot;
    protected Vector3 _mousePos;
    [SerializeField] protected Prop carriedObject;
    
//    [Header("Debug: ")] 
//    public GameObject controlsPanel;
//    public TextMeshProUGUI controlsText;
//    public bool showControls;
    
    
    // TO BE CHANGED!
    [Header("Temp: ")]
    [Space(2)]
    public Sprite sprtToSwap;
    public SpriteRenderer spriteRenderer;
    public GameObject rugtoMove;
    public Transform newRugLoc;
    public Prop objToSwap;


	void Update () {
	    
        if (isCarrying)
        {
            Inspect(carriedObject);
            CheckDrop();
            
            if (holdingMode)
            {
                HoldItem(carriedObject);
                PlaceObject();
            }
        }
        else
        {
            PickupObject();
        }
	    
	    //temp bugs to fix
	    if (interactingWithCanvas && Input.GetKeyDown(interactionKey))
	    {
	        exitCanvas();
	    }
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
                //Debug.Log(hit.collider);

                Prop pickupable = hit.collider.GetComponent<Prop>();
                //temp
                PillarButton button = hit.collider.GetComponent<PillarButton>();
                LightOrbScript orb = hit.collider.GetComponent<LightOrbScript>();
                WorldCanvasMenu canvas = hit.collider.GetComponent<WorldCanvasMenu>();
                MakeUIElement makeUiElement = hit.collider.GetComponent<MakeUIElement>();
                
                
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.green); // Drawing ray
                
                if(pickupable != null && 
                   Vector3.Distance(pickupable.transform.position, mainCamera.transform.position) < pickupDistance)
                {
                    // Getting item original Location (These are not being set to the right value)
                   // _objOriginalPos = pickupable.originalPos;
                    //_objOriginalRot = pickupable.originalRot;
                    //Debug.Log(" IO._objOriginalRot: " + pickupable.originalRot);
                    
                    
                    isCarrying = true;
                    carriedObject = pickupable;
                    carriedObject.transform.parent = gameObject.transform;
                    pickupable.GetComponent<Rigidbody>().isKinematic = true;
                    pickupable.amPickedUp = true;
                }
                if (button != null)
                {
                    button.amPressed = true;
                }
                if (orb != null && orb.objPlaced)
                {
                    orb.transform.Rotate(90,0,0);
                }
                if (canvas != null)
                {
                    CanvasInteraction();
                }

                if (makeUiElement != null)
                {
                    if (!makeUiElement.elementActive)
                    {
                       makeUiElement.EnableUI();
                    }
                    else
                    {
                        makeUiElement.DisableUI();
                    }
                }
            }
        }
    }
    
    // Filled with carriedObject in update
    public void Inspect(Prop obj) 
    {
        if(obj != null && !holdingMode)
        {
            // Change bool
            inspectionMode = true;
            
            //Move the object into position
            obj.transform.position = Vector3.Lerp(obj.transform.position,
                (mainCamera.transform.position + obj.propInspectOffset) + mainCamera.transform.forward * inspectDistance, Time.deltaTime * smoothing);
            
            // Object should face the player
            obj.transform.LookAt(gameObject.transform);
            
            // Lock Mouse Look
            rigidbodyFirstPersonController.enabled = false;
            
            // Unlock Mouse cursor
            rigidbodyFirstPersonController.mouseLook.SetCursorLock(false);
            
            //Setting the look rotation of the camera during while Inspecting
            //_inspectCameraRot *= Quaternion.Euler(0,0,0);
            mainCamera.transform.localEulerAngles = new Vector3(Mathf.Lerp(mainCamera.transform.localEulerAngles.x, 0, Time.deltaTime * rotSmoothing), 
                mainCamera.transform.localEulerAngles.y, mainCamera.transform.localEulerAngles.z);

            // Mode switching (Move to update?)
            if (Input.GetKeyDown(holdItemKey))
            {
                HoldingMode();
            }
            if (Input.GetKeyDown(swapItemKey))
            {
                SwapObject();
            }
            
            //Forgot how this worked, but can be used
            if (Input.GetMouseButtonDown(1))
            {
                RotateItem();
            }

        }   
    }

    public void CanvasInteraction()
    {
        interactingWithCanvas = true;
        
        // Lock Mouse Look
        rigidbodyFirstPersonController.enabled = false;
            
        // Unlock Mouse cursor
        rigidbodyFirstPersonController.mouseLook.SetCursorLock(false);     
    }

    public void exitCanvas()
    {
        
            interactingWithCanvas = false;
        
            // Lock Mouse Look
            rigidbodyFirstPersonController.enabled = true;
            
            // Unlock Mouse cursor
            rigidbodyFirstPersonController.mouseLook.SetCursorLock(true);
    }
    
    // Add to HoldItem()
    public void HoldingMode()
    {
        holdingMode = true;
        inspectionMode = false;
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
        
        // Changing Item Position (Should Props have custom hold Pos?)
        if (obj != null)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position,
                holdingGuide.position + mainCamera.transform.forward * holdingDistance, Time.deltaTime * smoothing);
            
            //_inspectItemRot *= Quaternion.Euler(0,0,0);
            obj.transform.localRotation = holdingGuide.transform.localRotation;
        }
    }

    void CheckDrop()
    {
        if (Input.GetKeyDown(dropObjectKey) && holdingMode)
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
            // Make sure bools are false
            isCarrying = false;
            holdingMode = false;
            inspectionMode = false;
            
            // Unlock the controller
            rigidbodyFirstPersonController.mouseLook.SetCursorLock(true);
            rigidbodyFirstPersonController.enabled = true;
            
            // Unparent the carriedObject & all physics interactions
            carriedObject.transform.parent = null;
            carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            
            
            carriedObject = null;
        }
    }

    public void PutBackObj()
    {
        if (carriedObject != null)
        {
            // Make sure bools are false
            isCarrying = false;
            holdingMode = false;
            inspectionMode = false;
            
            // Unlock the controller
            rigidbodyFirstPersonController.mouseLook.SetCursorLock(true);
            rigidbodyFirstPersonController.enabled = true;
            
            // Unparent the carriedObject & turn on all physics interactions
            carriedObject.transform.parent = null;
            carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            
            // Setting the carriedObject transform back to the original state that is stored in Prop. (transforms are nto being set correctly) 
            carriedObject.transform.position = carriedObject.originalPos;
            carriedObject.transform.eulerAngles = carriedObject.originalRot;
            
            // No longer carrying an object
            carriedObject = null;
        }
    }
    
    // Note: Should this be in a different script????
    public void PlaceObject()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit = new RaycastHit();
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
             
        if(Physics.Raycast(ray, out hit))
        {
            //Debug.Log(hit.collider);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue); // Drawing ray

            PlacementLocation pLoc = hit.collider.GetComponent<PlacementLocation>();
            // If the Object the ray hits is in the list of the Props valid locations
            // This should usually return false
            if (carriedObject != null && carriedObject.CheckLocation(pLoc, carriedObject.validLocations))
            {
                //change reticle
                Debug.Log("this is valid location");
                Debug.Log(carriedObject.CheckLocation(pLoc, carriedObject.validLocations));

                if (Input.GetMouseButtonDown(0))
                {
                    carriedObject.transform.position = hit.transform.position;
                    carriedObject.transform.eulerAngles = hit.transform.eulerAngles;
                    carriedObject.objPlaced = true;
                    
                    DropObject();
                }
            }
        }
    }
    // Temp
    public void SwapObject()
    {
        if (spriteRenderer != null || sprtToSwap != null)
        {
            spriteRenderer.sprite = sprtToSwap;
        }
        else
        {
            Debug.LogWarning("Null in the inspector");
        }
        
        //carriedObject = objToSwap; // then what?
    }
    
    // not used
    private void RotateItem()
    {
        // Setting Camera Rotation to Mouse Position     
        Vector3 mouseDelt = _mousePos - Input.mousePosition;
        carriedObject.transform.Rotate(new Vector3(mouseDelt.y, mouseDelt.x, 0));
       
        // Get Mouse position
        _mousePos = Input.mousePosition;
    }
}
