using UnityEngine;

public class FishCheckScript : MonoBehaviour
{
    public string fishLayerName = "isFish";
    private int fishLayer;

    [Header("LEAVE EMPTY")]
    public GameObject closesFish;

    void Start()
    {
        fishLayer = LayerMask.NameToLayer(fishLayerName);
    }

    void Update()
    {
        closesFish = fish();
    }

    public GameObject fish()
    {
        GameObject[] allFish = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject fish in allFish)
        {
            if (fish.layer == fishLayer)
            {
                // Calculate the distance between the bobber and the fish
                float distance = Vector3.Distance(transform.position, fish.transform.position);

                Debug.Log($"Distance to {fish.name}: {distance}");

                return fish;
            }
        }

        Debug.Log("No fish found in range");
        return null;
    }
}
