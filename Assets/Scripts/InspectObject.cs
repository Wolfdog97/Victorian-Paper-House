using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityStandardAssets.Characters.FirstPerson;

public class InspectObject : MonoBehaviour {

	GameObject mainCamera;
    GameObject carriedObject;

    public RigidbodyFirstPersonController rigidbodyFirstPersonController;
    

    public Transform holdingGuide;
    private Transform tempTrans;
    
    public bool isHolding;
    public bool holdingMode;

    public float inspectDistance = 1;
    public float pickupDistance = 3;
    public float holdingDistance = 2;
    public float smoothing = 10;

    private Quaternion inpectRot;
    public float rotSmoothing = 5;


	
	void Start () {
        mainCamera = GameObject.FindWithTag("MainCamera");
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
	}
    
    void Inspect(GameObject obj)
    {
        if(obj != null && !holdingMode)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position,
            mainCamera.transform.position + mainCamera.transform.forward * inspectDistance, Time.deltaTime * smoothing);
            
            // Lock MC
            rigidbodyFirstPersonController.enabled = false;
            
            inpectRot *= Quaternion.Euler(0,0,0);
            mainCamera.transform.rotation = inpectRot;
            //mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, inpectRot, Time.deltaTime * rotSmoothing);
            
            // Mode switching 

            if (Input.GetKeyDown(KeyCode.Q))
            {
                holdingMode = true;
            }
        }   
    }


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

    void HoldItem(GameObject item)
    {
        if (!rigidbodyFirstPersonController.enabled)
        {
            rigidbodyFirstPersonController.enabled = true;
        }
        
        if (item != null)
        {
            item.transform.position = Vector3.Lerp(item.transform.position,
                holdingGuide.position + mainCamera.transform.forward * holdingDistance, Time.deltaTime * smoothing);
            item.transform.rotation = holdingGuide.rotation;
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
            
            rigidbodyFirstPersonController.enabled = true;

            carriedObject.transform.parent = null;
            carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            carriedObject = null;
        }
    }
}
