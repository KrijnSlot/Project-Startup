using UnityEngine;
using System.Collections;

public class FishMovementScript : MonoBehaviour
{

    public float speed;
    //public float turnSpeed = 4.0f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    float neighborDistance = 3.0f;

    bool turning = false;

    // Use this for initialization
    void Start()
    {
        speed = Random.Range(3.0f, 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        ApplyTankBoundary();

        if (turning)
        {
            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                TurnSpeed() * Time.deltaTime);
            speed = Random.Range(0.5f, 1);
        }
        else
        {
            if (Random.Range(0, 5) < 1)
                ApplyRules();
        }

        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyTankBoundary()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) >= FishFlockScript.swimRange)
        {
            turning = true;
        }
        else
        {
            turning = false;
        }
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = FishFlockScript.allFish;

        Vector3 vCenter = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = FishFlockScript.goalPos;

        float dist;
        int groupSize = 0;

        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if (dist <= neighborDistance)
                {
                    vCenter += go.transform.position;
                    groupSize++;

                    if (dist < 0.75f)
                    {
                        vAvoid = vAvoid + (this.transform.position - go.transform.position);
                    }

                    FishMovementScript anotherFish = go.GetComponent<FishMovementScript>();
                    gSpeed += anotherFish.speed;
                }

            }
        }

        if (groupSize > 0)
        {
            vCenter = vCenter / groupSize + (goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vCenter + vAvoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction),
                    TurnSpeed() * Time.deltaTime);
            }
        }

    }

    float TurnSpeed()
    {
        return Random.Range(2.8f, 3.6f);
    }
}