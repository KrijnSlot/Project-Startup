using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class FishingRod : MonoBehaviour
{
    public PickUpScript mainCamPickUpScript;

    public bool isEquipped;
    public bool LookingAtWater;

    public bool isCasted;
    public bool pulled;

    Animator animator;
    public GameObject bobber;
    public GameObject end_of_rope;  // --- > IF USING ROPE
    public GameObject start_of_rope;   // --- > IF USING ROPE   
    public GameObject start_of_rod;    // --- > IF USING ROPE   

    Transform baitPosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isEquipped = true;
    }



    void Update()
    {
        if (mainCamPickUpScript.holdingRod)
        {
            myInputs();
        }
    }

    void myInputs()
    {
        if (isEquipped)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {

                if (hit.collider.CompareTag("FishingArea"))
                {
                    LookingAtWater = true;

                    if (Input.GetMouseButtonDown(0) && !isCasted && !pulled)
                    {
                        StartCoroutine(CastRod(hit.point));
                    }
                }
                else
                {
                    LookingAtWater = false;
                }

            }
            else
            {
                LookingAtWater = false;
            }
        }

        // --- > IF USING ROPE < --- //
        if (isCasted || pulled)
        {
            if (start_of_rope != null && start_of_rod != null && end_of_rope != null)
            {
                start_of_rope.transform.position = start_of_rod.transform.position;

                if (baitPosition != null)
                {
                    end_of_rope.transform.position = baitPosition.position;
                }
            }
            else
            {
                Debug.Log("MISSING ROPE REFERENCES");
            }
        }

        if (isCasted && Input.GetMouseButtonDown(1))
        {
            PullRod();
        }
    }

    IEnumerator CastRod(Vector3 targetPosition)
    {
        isCasted = true;
        animator.SetTrigger("Cast");

        // Create a delay between the animation and when the bait appears in the water
        yield return new WaitForSeconds(1f);

        GameObject instantiatedBait = Instantiate(bobber);
        instantiatedBait.transform.position = targetPosition;

        baitPosition = instantiatedBait.transform;

        // ---- > Start Fish Bite Logic
    }

    private void PullRod()
    {
        animator.SetTrigger("Pull");
        isCasted = false;
        pulled = true;

        // ---- > Start Minigame Logic
    }
}
