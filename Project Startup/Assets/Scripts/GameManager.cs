using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    [SerializeField] IntValue intValue;
    public static int money;
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
        money = 0;
        Debug.Log(intValue.value);
    }

    private void Update()
    {
        intValue.value -= 1;

        //Debug.Log("money: " + money);
    }

    public void RandomAddMoney(int min, int max)
    {
        int addedMoney = Random.Range(min, max);
        money += addedMoney;
    }
}
