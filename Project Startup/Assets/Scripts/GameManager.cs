using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static int money;

    private void Awake()
    {
        money = 0;
    }

    private void Update()
    {
        //Debug.Log("money: " + money);
    }

    public static void RandomAddMoney(int min, int max)
    {
        int addedMoney = Random.Range(min, max);
        money += addedMoney;
    }
}
