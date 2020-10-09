using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody rb;
    public Vector3[] positions;
    public float speed;

    private int actualPosition = 0;
    private int nextPosition = 1;

    public bool moveToTheNext = true;
    public float WaitTIme;

    public bool canGo;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canGo)
        movePlataform();
    }

    void movePlataform()
    {
        if(moveToTheNext)
        {
            StopCoroutine(WaitForMove(0));
            actualPosition = nextPosition;
            rb.MovePosition(Vector3.MoveTowards(rb.position, positions[nextPosition], speed * Time.deltaTime));
        }

        if(Vector3.Distance(rb.position, positions[nextPosition]) <= 0)
        {
            StartCoroutine(WaitForMove(WaitTIme));
            actualPosition = nextPosition;
            nextPosition++;

            if(nextPosition > positions.Length - 1)
            {
                nextPosition = 0;
            }
        }
    }

    IEnumerator WaitForMove(float time)
    {
        moveToTheNext = false;
        yield return new WaitForSeconds(time);
        moveToTheNext = true;
    }
}
