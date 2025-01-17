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
    public FishCheckScript bobberFishCheckScript;

    public bool isEquipped;
    public bool LookingAtWater;

    public bool isCasted;
    public bool pulled;

    Animator animator;
    public GameObject bobber;
    public GameObject end_of_rope;  // --- > IF USING ROPE
    public GameObject start_of_rope;   // --- > IF USING ROPE   
    public GameObject start_of_rod;    // --- > IF USING ROPE   

    private GameObject caughtFish;

    Transform baitPosition;

    private float timer;
    private float randomTime;
    private bool fishCaught;


    private void Start()
    {
        animator = GetComponent<Animator>();
        isEquipped = true;
        ResetTimer();
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
                Debug.Log("no rope reference in inspector");
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

        // Reset the timer and set a new random time when casting
        ResetTimer();

        // ---- > Start Fish Bite Logic
        while (isCasted)
        {
            // Increment the timer
            timer += Time.deltaTime;

            // Check if the timer exceeds or equals the random time
            if (timer >= randomTime)
            {
                Debug.Log("Random timer reached!");
                Debug.Log("catch");

                // Perform your desired action here, e.g., catch the fish
                caughtFish = bobberFishCheckScript.closesFish;
                fishCaught = true;
                // Reset the timer for the next cycle
                ResetTimer();

                // Break the loop to stop the coroutine once the fish is caught
                break;
            }

            yield return null; // Wait for the next frame
        }
    }

    private void PullRod()
    {
        animator.SetTrigger("Pull");
        isCasted = false;
        pulled = true;

        if (fishCaught)
        {
            // ---- > Start Minigame Logic

            fishCaught = false;
        }
    }

    void ResetTimer()
    {
        timer = 0f;
        randomTime = UnityEngine.Random.Range(2f, 7f); // Random value between 1 and 5 seconds
        Debug.Log($"New random time set: {randomTime} seconds");
    }
}


