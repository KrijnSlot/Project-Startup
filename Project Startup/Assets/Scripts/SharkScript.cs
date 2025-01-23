using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkScript : MonoBehaviour
{
    public GameObject boat;
    public float sharkSpeed = 5f;
    public LayerMask boatBitePoint;

    private bool inRange;

    Rigidbody rb;
    Vector3 directionToBoat;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; 
    }

    void Update()
    {
        MoveTowardBoat();
        CheckBoatDistance();
    }
    void CheckBoatDistance()
    {
        inRange = Physics.CheckSphere(transform.position, 1f, boatBitePoint);
        if (inRange) { Debug.Log("in range"); }
        else { Debug.Log("ah hell nah"); }
    }

    void MoveTowardBoat()
    {
        if (!inRange)
        {
            transform.LookAt(boat.transform);
            directionToBoat = (boat.transform.position - transform.position).normalized;
            rb.AddForce(directionToBoat * sharkSpeed, ForceMode.Force);
        }
        else
        {

/*            transform.LookAt(boat.transform);
            Vector3 xLock = transform.eulerAngles;
            xLock.x = 0f;
            transform.rotation = Quaternion.Euler(xLock);
            rb.AddForce(directionToBoat * (-sharkSpeed / 40), ForceMode.Force);*/
        }
    }
}
