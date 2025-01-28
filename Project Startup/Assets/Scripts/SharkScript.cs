using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkScript : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 5f; // Time to wait between attacks

    public GameObject boat; // Reference to the boat
    public float sharkSpeed = 5f; // Shark movement speed
    public LayerMask isBoat; // LayerMask to detect boat points
    public Animator animator;

    public int sharkHP = Random.Range(10, 15);

    private bool inRange; // Check if shark is in range of a boat point
    private Rigidbody rb;
    private GameObject closestPoint; // Closest boat point
    public GameObject[] allBoatPoints; // Array of all boat points

    private bool canAttack = true;
    private PointScript currentPointScript; // The PointScript of the currently occupied point
    public SharkSpawningScript spawningScript;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity for the shark

        // Find all boat points at runtime
        allBoatPoints = GameObject.FindGameObjectsWithTag("BoatPoint");

        if (allBoatPoints.Length == 0)
        {
            Debug.LogWarning("No boat points found! Make sure boat points are tagged as 'BoatPoint'.");
        }
    }

    void Update()
    {
        // If no point is currently occupied, find the closest available point
        if (currentPointScript == null || !currentPointScript.isOccupied)
        {
            closestPoint = FindClosestAvailablePoint();
        }

        MoveTowardBoat(); // Move toward the boat point
        CheckBoatDistance(); // Check distance to the boat point

        // Continuously update position if occupying a point
        if (currentPointScript != null && currentPointScript.isOccupied)
        {
            MaintainPositionAtPoint();
        }
    }

    public GameObject FindClosestAvailablePoint()
    {
        GameObject closestPoint = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject point in allBoatPoints)
        {
            if ((isBoat.value & (1 << point.layer)) != 0) // Check if the point is on the isBoat layer
            {
                PointScript pointScript = point.GetComponent<PointScript>();
                if (pointScript != null && !pointScript.isOccupied) // Ensure the point is not occupied
                {
                    float distance = Vector3.Distance(transform.position, point.transform.position);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        closestPoint = point;
                    }
                }
            }
        }

        return closestPoint;
    }

    void CheckBoatDistance()
    {
        if (closestPoint != null)
        {
            inRange = Vector3.Distance(transform.position, closestPoint.transform.position) <= 1f;
        }
    }

    void MoveTowardBoat()
    {
        if (closestPoint != null)
        {
            transform.LookAt(boat.transform.position);
            if (!inRange)
            {
                // Move toward the closest point
                Vector3 directionToPoint = (closestPoint.transform.position - transform.position).normalized;
                rb.AddForce(directionToPoint * sharkSpeed, ForceMode.Force);
            }
            else
            {
                // Snap to the closest point
                SnapToClosestPoint();
            }
        }
    }

    void SnapToClosestPoint()
    {
        PointScript pointScript = closestPoint.GetComponent<PointScript>();
        if (pointScript != null && !pointScript.isOccupied)
        {
            // Occupy the point
            pointScript.isOccupied = true;
            currentPointScript = pointScript; // Assign the current point script

            transform.position = closestPoint.transform.position;
            transform.rotation = closestPoint.transform.rotation;

            if (canAttack)
            {
                canAttack = false; // Prevent further attacks during cooldown
                animator.SetTrigger("Attack");

                // Do damage to the boat down here

                // Start the cooldown coroutine
                StartCoroutine(AttackCooldown(pointScript));
            }
            else
            {
                Debug.Log("Attack on cooldown.");
            }
        }
    }

    void MaintainPositionAtPoint()
    {
        if (currentPointScript != null)
        {
            // Continuously maintain the shark's position and rotation at the occupied point
            transform.position = closestPoint.transform.position;
            transform.rotation = closestPoint.transform.rotation;
        }
    }

    IEnumerator AttackCooldown(PointScript pointScript)
    {
        Debug.Log("Attack started cooldown.");
        yield return new WaitForSeconds(attackCooldown);

        Debug.Log("Attack ready again.");
        canAttack = true; // Reset attack availability

        // Release the point after cooldown
        if (pointScript != null)
        {
            pointScript.isOccupied = false;
            currentPointScript = null; // Clear the current point script reference
        }
    }

    public void SharkHit(int damage)
    {
        sharkHP -= damage;

        if (sharkHP <= 0)
        {
            // Remove this shark from the sharks list
            //spawningScript.sharks.Remove();

            // Destroy the shark game object
            Destroy(gameObject);
        }
    }


    private void OnDestroy()
    {
        // If the shark is destroyed, release the currently occupied point
        if (currentPointScript != null)
        {
            currentPointScript.isOccupied = false;
        }
    }
}
