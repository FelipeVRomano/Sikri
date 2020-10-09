using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject ObjectToPickUp;
    public GameObject PickedObject;
    public Transform interactionZone;

    private float armazenaJumpForce;
    private Moviment movimento;
    void Start()
    {
        movimento = GetComponent<Moviment>();
        
    }
    void Update()
    {
       if(ObjectToPickUp != null && PickedObject == null && ObjectToPickUp.GetComponent<PickableObject>().isPickable)
        { 
                if(Input.GetButtonDown("Interagir"))
                {
                    armazenaJumpForce = movimento.jumpForce;
                    movimento.jumpForce = 0;
                    PickedObject = ObjectToPickUp;
                    PickedObject.transform.SetParent(interactionZone);
                    PickedObject.transform.position = interactionZone.position;
                    PickedObject.GetComponent<Rigidbody>().useGravity = false;
                    PickedObject.GetComponent<Rigidbody>().isKinematic = true;
                }
            
        }

       else if(PickedObject != null)
        {
            if (Input.GetButtonDown("Interagir"))
            {
                movimento.jumpForce = armazenaJumpForce;
                PickedObject.transform.SetParent(null);
                PickedObject.GetComponent<Rigidbody>().useGravity = true;
                PickedObject.GetComponent<Rigidbody>().isKinematic = false;               
                PickedObject = null;
            }
        }
    }
}
