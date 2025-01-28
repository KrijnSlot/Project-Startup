using UnityEngine;

public class FishCheckScript : MonoBehaviour
{
    public static FishCheckScript instance;

    public float maxFishDistance = 5;
    public string fishLayerName = "isFish";
    private int fishLayer;

    [HideInInspector] public bool canBeCaught;

    [HideInInspector] public GameObject closesFish;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        fishLayer = LayerMask.NameToLayer(fishLayerName);
    }

    void Update()
    {
        closesFish = FindClosestFish();
    }

    public GameObject FindClosestFish()
    {
        GameObject[] allFish = GameObject.FindObjectsOfType<GameObject>();
        GameObject closestFish = null;
        float shortestDistance = maxFishDistance; // Start with the maximum allowable distance

        foreach (GameObject fish in allFish)
        {
            if (fish.layer == fishLayer)
            {
                // Calculate the distance between the bobber and the fish
                float distance = Vector3.Distance(transform.position, fish.transform.position);

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestFish = fish;
                }
            }
        }

        if (closestFish != null)
        {
            canBeCaught = true;
            Debug.Log($"Closest fish is {closestFish.name} at distance {shortestDistance}");
        }
        else
        {
            canBeCaught = false;
            Debug.Log("No fish within range.");
        }

        return closestFish;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FishingArea"))
        {
            Rigidbody bobberRb = this.GetComponent<Rigidbody>();

            // Lock movement on the x and z axes by applying constraints.
            bobberRb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

            Debug.Log("Bobber entered the FishingArea and x/z axes are locked.");
        }
    }

}
