using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SharkSpawningScript : MonoBehaviour
{
    public int maxSharkAmount = 2;
    public GameObject shark;
    public List<GameObject> sharks = new List<GameObject>();

    private Vector3 pos;

    void Start()
    {
        StartCoroutine(SpawnSharkRoutine());
    }

    IEnumerator SpawnSharkRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(0f, 5f);
            yield return new WaitForSeconds(waitTime);

            SpawnShark();
        }
    }

    void SpawnShark()
    {
        if (sharks.Count < maxSharkAmount)
        {
            randomizePos();
            GameObject enemyShark = Instantiate(shark, pos, Quaternion.identity);
            sharks.Add(enemyShark);
        }
    }

    void randomizePos()
    {
        pos = new Vector3(
            Random.Range(-FishFlockScript.swimRange, FishFlockScript.swimRange),
            Random.Range(-2, -0.5f),
            Random.Range(-FishFlockScript.swimRange, FishFlockScript.swimRange)
        );
    }
}
