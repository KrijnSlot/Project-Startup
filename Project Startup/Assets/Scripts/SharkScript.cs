using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkScript : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 5f;

    public GameObject boat; 
    public float sharkSpeed = 5f; 
    public LayerMask isBoat; 
    public Animator animator;

    public int sharkHP;

    private bool inRange; 
    private Rigidbody rb;
    private GameObject closestPoint; 
    public GameObject[] allBoatPoints;

    private bool canAttack = true;
    private PointScript currentPointScript;
    public SharkSpawningScript spawningScript;

    [Header("The damage the shark gets")]
    public int damage = 1;

    void Start()
    {
        sharkHP = Random.Range(2, 5);
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; 

        allBoatPoints = GameObject.FindGameObjectsWithTag("BoatPoint");
        spawningScript = FindObjectOfType<SharkSpawningScript>();
    }

    void Update()
    {
        if (currentPointScript == null || !currentPointScript.isOccupied)
        {
            closestPoint = FindClosestAvailablePoint();
        }

        MoveTowardBoat();
        CheckBoatDistance();

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
            if ((isBoat.value & (1 << point.layer)) != 0)
            {
                PointScript pointScript = point.GetComponent<PointScript>();
                if (pointScript != null && !pointScript.isOccupied)
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
                Vector3 directionToPoint = (closestPoint.transform.position - transform.position).normalized;
                rb.AddForce(directionToPoint * sharkSpeed, ForceMode.Force);
            }
            else
            {
                SnapToClosestPoint();
            }
        }
    }

    void SnapToClosestPoint()
    {
        PointScript pointScript = closestPoint.GetComponent<PointScript>();
        if (pointScript != null && !pointScript.isOccupied)
        {
            pointScript.isOccupied = true;
            currentPointScript = pointScript; 

            transform.position = closestPoint.transform.position;
            transform.rotation = closestPoint.transform.rotation;

            if (canAttack)
            {
                canAttack = false; 
                animator.SetTrigger("Attack");

                BoatScript.DoDamage2Boat(50);

                StartCoroutine(AttackCooldown(pointScript));
            }
        }
    }

    void MaintainPositionAtPoint()
    {
        if (currentPointScript != null)
        {
            transform.position = closestPoint.transform.position;
            transform.rotation = closestPoint.transform.rotation;
        }
    }

    IEnumerator AttackCooldown(PointScript pointScript)
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true; 

        if (pointScript != null)
        {
            pointScript.isOccupied = false;
            currentPointScript = null; 
        }
    }

    public void SharkHit()
    {
        sharkHP -= damage;

        if (sharkHP <= 0)
        {
            spawningScript.sharks.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (currentPointScript != null)
        {
            currentPointScript.isOccupied = false;
        }
    }
}
