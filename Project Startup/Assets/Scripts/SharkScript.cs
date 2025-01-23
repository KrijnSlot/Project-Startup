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

    private bool inRange; // Check if shark is in range of a boat point
    private Rigidbody rb;
    private GameObject closestPoint; // Closest boat point
    public GameObject[] allBoatPoints; // Array of all boat points

    private bool canAttack = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity for the shark
    }

    void Update()
    {
        closestPoint = FindClosestPoint(); // Find the closest boat point
        MoveTowardBoat(); // Move toward the boat point
        CheckBoatDistance(); // Check distance to the boat point
    }

    public GameObject FindClosestPoint()
    {
        GameObject closestPoint = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject point in allBoatPoints)
        {
            if ((isBoat.value & (1 << point.layer)) != 0) // Check if the point is on the isBoat layer
            {
                float distance = Vector3.Distance(transform.position, point.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestPoint = point;
                }
            }
        }

        /*        if (closestPoint != null)
                {
                    Debug.Log($"Closest point is {closestPoint.name} at distance {shortestDistance}");
                }
                else
                {
                    Debug.Log("No point within range.");
                }*/

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
        transform.position = closestPoint.transform.position;
        transform.rotation = closestPoint.transform.rotation;

        if (canAttack)
        {
            canAttack = false; // Prevent further attacks during cooldown
            animator.SetTrigger("Attack");


            // Do damage to the boat down here



            // Start the cooldown coroutine
            StartCoroutine(AttackCooldown());
        }
        else
        {
            Debug.Log("Attack on cooldown.");
        }
    }

    IEnumerator AttackCooldown()
    {
        Debug.Log("Attack started cooldown.");
        yield return new WaitForSeconds(attackCooldown);
        Debug.Log("Attack ready again.");
        canAttack = true; // Reset attack availability
    }
}
