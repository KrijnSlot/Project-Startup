using Boxophobic.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoatScript : MonoBehaviour
{
    [SerializeField] private int boatHP = 100;
    [SerializeField] private Slider slider;
    private int boatMaxHP;

    private static BoatScript instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        boatMaxHP = boatHP;
        slider.maxValue = boatMaxHP;
        slider.value = boatHP;
    }

    void Update()
    {
        slider.value = boatHP;
        GameOver();
    }

    public static void DoDamage2Boat(int damage)
    {
        if (instance != null)
        {
            int newDamage = Random.Range(damage / 2, damage);
            instance.boatHP -= newDamage;
            instance.boatHP = Mathf.Clamp(instance.boatHP, 0, instance.boatMaxHP);
        }
    }

    void GameOver()
    {
        if (boatHP == 0)
        {
            Debug.Log("ENDSCENE");
            SceneManager.LoadScene("ENDSCENE");
        }
    }
}