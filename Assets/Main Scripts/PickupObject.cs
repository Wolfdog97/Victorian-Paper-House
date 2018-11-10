using UnityEngine;

public class PickupObject : MonoBehaviour {

    GameObject mainCamera;
    GameObject carriedObject;
    public bool carrying;
    public float distance;
    public float pickupDistance = 3;
    public float smoothing;


	
	void Start () {
        mainCamera = GameObject.FindWithTag("MainCamera");
	   
	    
	}
	
	
	void Update () {
        if (carrying)
        {
            carry(carriedObject);
            checkDrop();
        }
        else
        {
            pickup();
        }
	}
    
    void carry(GameObject obj)
    {
        if(obj != null)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position,
            mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smoothing);
        }
        
    }

    void pickup()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
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
                    carrying = true;
                    carriedObject = pickupable.gameObject;
                    pickupable.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }

    void checkDrop()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
        {
            dropObject();
        }
    }

    void dropObject()
    {
        if (carriedObject != null)
        {
            carrying = false;
            carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            carriedObject = null;
        }
    }
}
