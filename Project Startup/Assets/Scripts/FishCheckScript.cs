using UnityEngine;

public class BobberFishDistance : MonoBehaviour
{
    public string fishLayerName = "isFish";
    private int fishLayer;

    void Start()
    {
        fishLayer = LayerMask.NameToLayer(fishLayerName);
    }

    void Update()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == fishLayer)
            {
                // Calculate the distance between the bobber and the fish
                float distance = Vector3.Distance(transform.position, obj.transform.position);

                Debug.Log($"Distance to {obj.name}: {distance}");
            }
        }
    }
}
