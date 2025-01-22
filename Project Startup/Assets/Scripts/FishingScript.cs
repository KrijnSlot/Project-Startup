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
    public bool timerDone;
    public bool fishCaught;

    Animator animator;
    public GameObject bobber;
    public GameObject rope;

    private GameObject caughtFish;

    Transform baitPosition;

    private float timer;
    private float randomTime;

    //private GameObject instantiatedBait;

    private void Start()
    {
        rope.SetActive(false);
        bobber.SetActive(false);
        animator = GetComponent<Animator>();
        isEquipped = false;
        ResetTimer();
    }

    void Update()
    {
        if (mainCamPickUpScript.holdingRod)
        {
            myInputs();
            isEquipped = true;
        }
        else { isEquipped = false; }
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

        if (isCasted && Input.GetMouseButtonDown(1))
        {
            PullRod();
        }
    }

    IEnumerator CastRod(Vector3 targetPosition)
    {
        fishCaught = false;
        isCasted = true;
        animator.SetTrigger("Cast");

        // Create a delay between the animation and when the bait appears in the water
        yield return new WaitForSeconds(1f);

        rope.SetActive(true);
        bobber.SetActive(true);
        //instantiatedBait = Instantiate(bobber);
        bobber.transform.position = targetPosition;

        baitPosition = bobber.transform;

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
                timerDone = true;
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

        if (timerDone && pulled)
        {
            timerDone = false;
            fishCaught = true;
        }
        else if (!timerDone && pulled)
        {
            isCasted = false;
        }

        if (fishCaught)
        {
            GameManager.RandomAddMoney(10, 100);
            bool miniGameDone = false;
            // ---- > Start Minigame Logic
            if (miniGameDone) 
            {
                fishCaught = false; 

            }
        }

        pulled = false;
        rope.SetActive(false);
        bobber.SetActive(false);
    }

    void ResetTimer()
    {
        timer = 0f;
        randomTime = UnityEngine.Random.Range(2f, 6f); // Random value between 1 and 5 seconds
        Debug.Log($"New random time set: {randomTime} seconds");
    }
}


