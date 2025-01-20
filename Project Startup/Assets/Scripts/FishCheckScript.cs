using UnityEngine;

public class FishCheckScript : MonoBehaviour
{
    public float maxFishDistance = 5;
    public string fishLayerName = "isFish";
    private int fishLayer;

    [HideInInspector] public GameObject closesFish;

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
            Debug.Log($"Closest fish is {closestFish.name} at distance {shortestDistance}");
        }
        else
        {
            Debug.Log("No fish within range.");
        }

        return closestFish;
    }
}
