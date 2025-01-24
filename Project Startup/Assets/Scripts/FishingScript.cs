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

    Animator animator;
    public GameObject bobberPrefab;
    public GameObject rope;

    private GameObject caughtFish;

    Transform baitPosition;

    private float timer;
    private float randomTime;

    public Transform castPoint; // Point where the bobber will be launched from (e.g., player's hand or rod tip).
    public float castForce = 15f;

    //private GameObject instantiatedBait;

    private void Start()
    {
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
                if (Input.GetMouseButtonDown(0) && !isCasted && !pulled)
                {
                    StartCoroutine(CastRod(hit.point));
                }
            }
        }

        if (isCasted && Input.GetMouseButtonDown(1))
        {
            PullRod();
        }
    }

    IEnumerator CastRod(Vector3 targetPoint)
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
        bobberRb.AddForce(castDirection * castForce, ForceMode.VelocityChange);

        // Wait for a moment to simulate the cast action.
        yield return new WaitForSeconds(0.5f);

        // Optional: Add a condition to reset or pull the bobber later.
        pulled = false;
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
            gameManager.RandomAddMoney(10, 100);
            bool miniGameDone = false;
            // ---- > Start Minigame Logic
            if (miniGameDone)
            {
                fishCaught = false;

            }
        }

        pulled = false;
        rope.SetActive(false);
        bobberPrefab.SetActive(false);
    }

    void ResetTimer()
    {
        timer = 0f;
        randomTime = UnityEngine.Random.Range(2f, 6f); // Random value between 1 and 5 seconds
        Debug.Log($"New random time set: {randomTime} seconds");
    }
    GameManager gameManager => GameManager.Instance;
}


