using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkScript : MonoBehaviour
{
    public GameObject boat;
    public float sharkSpeed = 5f;
    public LayerMask isBoat;

    private bool inRange;
    private bool animationPlaying;

    [HideInInspector] public GameObject closesPoint;
    public GameObject[] allBoatPoints;

    Rigidbody rb;
    Vector3 directionToBoat;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Update()
    {
        closesPoint = FindClosestPoint();
        MoveTowardBoat();
        CheckBoatDistance();
        Debug.Log(closesPoint);
    }

    public GameObject FindClosestPoint()
    {
        Debug.Log("findclosespoint");
        GameObject[] allBoatPoints = GameObject.FindObjectsOfType<GameObject>();
        GameObject closesPoint = null;
        float shortestDistance = 1f; // Start with the maximum allowable distance

        foreach (GameObject point in allBoatPoints)
        {
            Debug.Log(allBoatPoints);
            if (point.layer == isBoat)
            {
                Debug.Log("if");
                // Calculate the distance between the bobber and the fish
                float distance = Vector3.Distance(transform.position, point.transform.position);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closesPoint = point;
                }
            }
        }

        if (closesPoint != null)
        {
            Debug.Log($"Closest point is {closesPoint.name} at distance {shortestDistance}");
        }
        else
        {
            Debug.Log("No point within range.");
        }

        return closesPoint;
    }

    void CheckBoatDistance()
    {
        inRange = Physics.CheckSphere(transform.position, 1f, isBoat);
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
            PlayAnimation();

            /*            transform.LookAt(boat.transform);
                        Vector3 xLock = transform.eulerAngles;
                        xLock.x = 0f;
                        transform.rotation = Quaternion.Euler(xLock);
                        rb.AddForce(directionToBoat * (-sharkSpeed / 40), ForceMode.Force);*/
        }
    }

    void PlayAnimation()
    {
        if (closesPoint != null)
        {
            transform.position = closesPoint.transform.position;
        }
    }
}
