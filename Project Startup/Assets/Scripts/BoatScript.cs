using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatScript : MonoBehaviour
{
    [SerializeField] private int boatHP;
    private int boatMaxHP;

    public int sliderBoatHP;

    void Start()
    {
        boatMaxHP = boatHP;
        sliderBoatHP = boatHP;
    }

    // Update is called once per frame
    void Update()
    {
        sliderBoatHP = boatHP / boatMaxHP;
    }
}
