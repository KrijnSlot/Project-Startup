using System.Collections;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public PickUpScript mainCamPickUpScript;
    public FishCheckScript bobberFishCheckScript;


    public bool isEquipped;

    public bool isCasted;
    public bool pulled;
    public bool timerDone;
    public bool fishCaught;
    private bool canClick;

    Animator animator;
    public GameObject bobberPrefab;
    public GameObject rope;

    private GameObject caughtFish;

    Transform baitPosition;

    private float timer;
    private float randomTime;

    public Transform castPoint; // Point where the bobber will be launched from (e.g., player's hand or rod tip).
    public float castForce = 15f;

    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

    InputEvents inputEvents => InputEvents.instance;

    //private GameObject instantiatedBait;

    private void Start()
    {
        canClick = true;
        inputEvents.fishingAction += MyInputs;

        rope.SetActive(false);
        bobberPrefab.SetActive(false);
        animator = GetComponent<Animator>();
        isEquipped = false;
        ResetTimer();
    }

    void Update()
    {
        if (mainCamPickUpScript.holdingRod)
        {
            ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            isEquipped = true;
            SkillcheckTester();
        }
        else { isEquipped = false; }
        //Debug.Log("minigame in fishing script is done: " + FishMinigameScript.instance.skillcheckIsDone);
        //Debug.Log("fish is caught: " + fishCaught);
    }

    void MyInputs()
    {
        if (!canClick) return;
        canClick = false;


        if (isCasted)
        {
            PullRod();
            StartCoroutine(ClickCooldown(0.2f));
            return;
        }

        if (isEquipped)
        {
            if (!isCasted && !pulled)
            {
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Debug.Log(hit.point);
                    CastRod(hit.point);
                }
            }
        }

        StartCoroutine(ClickCooldown(0.2f));
    }

    void CastRod(Vector3 targetPoint)
    {
        isCasted = true;

        // Ensure the rope and bobber are active.
        rope.SetActive(true);

        bobberPrefab.SetActive(true);

        // Get the Rigidbody component of the existing bobber.
        Rigidbody bobberRb = bobberPrefab.GetComponent<Rigidbody>();

        // Ensure the Rigidbody is properly set up.
        bobberRb.isKinematic = false; // Ensure it's not static.
        bobberRb.useGravity = true;   // Enable gravity for realism.
        bobberRb.constraints = RigidbodyConstraints.None; // Remove constraints.

        // Reset the bobber's position to the cast point.
        bobberPrefab.transform.position = castPoint.position;

        // Calculate the direction to cast the bobber.
        Vector3 castDirection = (targetPoint - castPoint.position).normalized;


        // Apply force to the bobber in the direction of the cast.
        bobberRb.velocity = Vector3.zero; // Reset any previous velocity.
        //yield return new WaitForSeconds(0); shit does not even work well
        bobberRb.AddForce(castDirection * castForce, ForceMode.VelocityChange);

        // Wait for a moment to simulate the cast action.

        // Optional: Add a condition to reset or pull the bobber later.
        pulled = false;
    }

    private IEnumerator ClickCooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
        canClick = true;
    }

    // Lock x and z movement when the bobber hits the "FishingArea" tag.
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FishingArea"))
        {
            Rigidbody bobberRb = bobberPrefab.GetComponent<Rigidbody>();

            // Lock movement on the x and z axes by applying constraints.
            bobberRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

            Debug.Log("Bobber entered the FishingArea and x/z axes are locked.");
        }
    }



    /*    IEnumerator CastRod(Vector3 targetPosition)
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
        }*/



    private void PullRod()
    {
        animator.SetTrigger("Pull");
        isCasted = false;
        pulled = true;

        if (/*timerDone &&*/ pulled)
        {
            //Debug.Log("you fucking did it bitchdickwod");
            timerDone = false;
            fishCaught = true;
        }
        /*else if (!timerDone && pulled)
        {
            isCasted = false;
        }*/

        pulled = false;
        rope.SetActive(false);
        bobberPrefab.SetActive(false);
    }

    private void SkillcheckTester()
    {
        if (fishCaught)
        {
            //gameManager.RandomAddMoney(10, 100);

            FishMinigameScript.instance.skillCheckUI.SetActive(true);
            // ---- > Start Minigame Logic
            if (FishMinigameScript.instance.skillcheckIsDone == true)
            {
                FishMinigameScript.instance.skillCheckUI.SetActive(false);
                fishCaught = false;
                FishMinigameScript.instance.skillcheckIsDone = false;
            }
        }
    }

    void ResetTimer()
    {
        timer = 0f;
        randomTime = UnityEngine.Random.Range(2f, 6f); // Random value between 1 and 5 seconds
        Debug.Log($"New random time set: {randomTime} seconds");
    }
    GameManager gameManager => GameManager.Instance;
}


