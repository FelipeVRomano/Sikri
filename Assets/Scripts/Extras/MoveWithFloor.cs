using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithFloor : MonoBehaviour
{
    CharacterController player;

    Vector3 groundPosition;
    Vector3 lastGroundPosition;
    string groundName;
    string lastGroundName;

    Quaternion actualRot;
    Quaternion lastRot;

    [SerializeField]
    private Transform transformDois;

    private BoxCollider boxCollider;

    private LayerMask mask;

    [SerializeField]
    private float centroQuadrado;

    void Start()
    {
        player = GetComponent<CharacterController>();

        if(player == null)
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        mask = LayerMask.GetMask("Default");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (player.isGrounded)
            {
                RaycastHit hit;


                if (Physics.SphereCast(transformDois.transform.position, player.height / 4.2f, -transform.up, out hit,5, mask))
                {
                    GameObject groundedIn = hit.collider.gameObject;
                    groundName = groundedIn.name;
                    groundPosition = groundedIn.transform.position;

                   // if (groundedIn != null)
                   //     print(groundedIn.name);
                    actualRot = groundedIn.transform.rotation;
                    if (groundPosition != lastGroundPosition && groundName == lastGroundName)
                    {
                        transform.position += groundPosition - lastGroundPosition;
                        
                        //Solucion
                        player.enabled = false;
                        player.transform.position = transform.position;
                        player.enabled = true;
                    }

                    if (actualRot != lastRot && groundName == lastGroundName)
                    {
                        var newRot = transform.rotation * (actualRot.eulerAngles - lastRot.eulerAngles);
                        transform.RotateAround(groundedIn.transform.position, Vector3.up, newRot.y);
                    }

                    lastGroundName = groundName;
                    lastGroundPosition = groundPosition;
                    lastRot = actualRot;
                }


            }


            else if (!player.isGrounded)
            {
                lastGroundName = null;
                lastGroundPosition = Vector3.zero;
                lastRot = Quaternion.Euler(0, 0, 0);
            }
        }

        else
        {
            RaycastHit hit;


            if (Physics.SphereCast(transformDois.transform.position, boxCollider.size.y / centroQuadrado, -transform.up, out hit, 5, mask))
            {
                GameObject groundedIn = hit.collider.gameObject;
                groundName = groundedIn.name;
                groundPosition = groundedIn.transform.position;

                if (groundedIn != null)
                    //print(groundedIn.name);
                actualRot = groundedIn.transform.rotation;
                if (groundPosition != lastGroundPosition && groundName == lastGroundName)
                {
                    transform.position += groundPosition - lastGroundPosition;
                }

                if (actualRot != lastRot && groundName == lastGroundName)
                {
                    var newRot = transform.rotation * (actualRot.eulerAngles - lastRot.eulerAngles);
                    transform.RotateAround(groundedIn.transform.position, Vector3.up, newRot.y);
                }

                lastGroundName = groundName;
                lastGroundPosition = groundPosition;
                lastRot = actualRot;
            }

        }
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            player = GetComponent<CharacterController>();
            Gizmos.DrawWireSphere(transformDois.transform.position, player.height / 4.2f);
        }
    }
}
