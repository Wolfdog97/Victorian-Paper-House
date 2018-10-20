using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Serialization;
using UnityStandardAssets.Characters.FirstPerson;
/* Missing or Broken:
 * Selecting tabs
 * get delta of mouse pos and add that to obj rotation 
 */

public class InspectObject : MonoBehaviour {

	public Camera mainCamera;
    GameObject carriedObject;

    public RigidbodyFirstPersonController rigidbodyFirstPersonController;
    

    public Transform holdingGuide;
    private Transform tempTrans;
    private Vector3 mousePos;
    
    public bool isHolding;
    public bool holdingMode;

    public float inspectDistance = 1;
    public float pickupDistance = 3;
    public float holdingDistance = 2;
    public float smoothing = 10;

    private Quaternion inspectCameraRot;
    private Quaternion inspectItemRot;
    public float rotSmoothing = 5;
    public float rotSpeed = 8;
    
	void Start ()
	{
	   
	}
	
	
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
	    
	    Debug.Log(rigidbodyFirstPersonController.mouseLook.lockCursor);
	}
    
    void Inspect(GameObject obj)
    {
        if(obj != null && !holdingMode)
        {
            //Move the object into position
            obj.transform.position = Vector3.Lerp(obj.transform.position,
            mainCamera.transform.position + mainCamera.transform.forward * inspectDistance, Time.deltaTime * smoothing);
            
            // Lock Mouse Look
            rigidbodyFirstPersonController.enabled = false;
            
            // Unlock Mouse cursor
            rigidbodyFirstPersonController.mouseLook.SetCursorLock(false);
            
            //Setting the look rotation of the camera during while Inspecting 
            inspectCameraRot *= Quaternion.Euler(0,0,0);
            //mainCamera.transform.rotation = inspectCameraRot;
            mainCamera.transform.localEulerAngles = new Vector3(Mathf.Lerp(mainCamera.transform.localEulerAngles.x, 0, Time.deltaTime * rotSmoothing), 
                mainCamera.transform.localEulerAngles.y, mainCamera.transform.localEulerAngles.z);
            //Quaternion.Lerp(mainCamera.transform.rotation,Quaternion.identity, Time.deltaTime * rotSmoothing);
            
            //Set Starting Item Rotation 
            //obj.transform.rotation = inspectItemRot;
            //inspectItemRot *= Quaternion.Euler(0,0,0);
            
            
            
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

                Pickupable pickupable = hit.collider.GetComponent<Pickupable>();
                if(pickupable != null && Vector3.Distance(pickupable.gameObject.transform.position, mainCamera.transform.position) < pickupDistance)
                {
                    isHolding = true;
                    carriedObject = pickupable.gameObject;
                    pickupable.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    
                    carriedObject.transform.parent = gameObject.transform;
                }
            }
        }
    }
    
    //(For rewrite) State
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
            
            inspectItemRot *= Quaternion.Euler(0,0,0);
            item.transform.rotation = inspectItemRot;
            
        }
    }

    void checkDrop()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dropObject();
        }
    }

    void dropObject()
    {
        if (carriedObject != null)
        {
            isHolding = false;
            holdingMode = false;
            
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
    }

    // Broken
    void RotateItem()
    {
        // Setting Camera Rotation to Mouse Position     
        Vector3 mouseDelt = mousePos - Input.mousePosition;
        carriedObject.transform.Rotate(new Vector3(mouseDelt.y, mouseDelt.x, 0));
       
        // Get Mouse position
        mousePos = Input.mousePosition;
    }
}
