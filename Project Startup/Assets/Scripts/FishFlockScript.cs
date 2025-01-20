using UnityEngine;
using System.Collections;

public class FishFlockScript : MonoBehaviour
{
    public GameObject fishPrefab;
    public GameObject goalPrefab;
    public static float swimRange = 3;

    [SerializeField]static int numFish = 5;
    public static GameObject[] allFish = new GameObject[numFish];
    public static Vector3 goalPos = Vector3.zero;

    void Start()
    {
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-swimRange, swimRange), Random.Range(1, 20), Random.Range(-swimRange, swimRange));

            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
        }
    }

    void Update()
    {
        HandleGoalPos();
    }

    void HandleGoalPos()
    {
        if (Random.Range(1, 10000) < 50)
        {
            goalPos = new Vector3(Random.Range(-swimRange, swimRange), Random.Range(1, 20), Random.Range(-swimRange, swimRange));
            goalPrefab.transform.position = goalPos;
        }
    }
}