using GogoGaga.OptimizedRopesAndCables;
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

    private Rope ropeScript;

    Transform baitPosition;

    private float timer;
    private float randomTime;

    public Transform castPoint; 
    public float castForce = 15f;

    Ray ray;

    InputEvents inputEvents => InputEvents.instance;

    private void Start()
    {
        ropeScript = FindObjectOfType<Rope>();
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
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

            if (FishCheckScript.instance.canBeCaught)
                SkillcheckTester();
        }
        else { isEquipped = false; }
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
        Debug.Log("is equipped: " + isEquipped);


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
        
        ropeScript.ropeLength = 1;

        rope.SetActive(true);

        bobberPrefab.SetActive(true);
        Rigidbody bobberRb = bobberPrefab.GetComponent<Rigidbody>();

        bobberRb.isKinematic = false; 
        bobberRb.useGravity = true; 
        bobberRb.constraints = RigidbodyConstraints.None;

        bobberPrefab.transform.position = castPoint.position;
        Vector3 castDirection = (targetPoint - castPoint.position).normalized;
        bobberRb.velocity = Vector3.zero; 
        bobberRb.AddForce(castDirection * castForce, ForceMode.VelocityChange);

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

    private void PullRod()
    {
        animator.SetTrigger("Pull");
        isCasted = false;
        pulled = true;

        if (/*timerDone &&*/ pulled && FishCheckScript.instance.canBeCaught)
        {
            //Debug.Log("you fucking did it bitchdickwod");
            timerDone = false;
            fishCaught = true;
        }
        else if (/*!timerDone && */pulled && !fishCaught)
        {
            isCasted = false;
            pulled = false;
            rope.SetActive(false);
            bobberPrefab.SetActive(false);
        }
    }

    private void SkillcheckTester()
    {
        if (fishCaught)
        {
            //gameManager.RandomAddMoney(10, 100);

            FishMinigameScript.instance.skillCheckUI.SetActive(true);
            if (FishMinigameScript.instance.skillcheckIsDone == true)
            {
                FishMinigameScript.instance.skillCheckUI.SetActive(false);
                fishCaught = false;
                FishMinigameScript.instance.skillcheckIsDone = false;

                pulled = false;
                rope.SetActive(false);
                bobberPrefab.SetActive(false);
            }
        }
    }

    void ResetTimer()
    {
        timer = 0f;
        randomTime = UnityEngine.Random.Range(2f, 6f);
        Debug.Log($"New random time set: {randomTime} seconds");
    }
    GameManager gameManager => GameManager.Instance;
}


