//using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TemporaryFishMinigameScript : MonoBehaviour
{
    [SerializeField] private Slider skillCheckSlider; // Reference to the UI Slider
    [SerializeField] private RectTransform targetRangeImage;
    private float movingLineValue, randomLineBorder, topPivotSlider, bottemPivotSlider;
    private bool lineIsMoving, valueMovingUp;
    [SerializeField] float velocity = 0.01f;
    float randomPosition;
    float clampedRange;
    float allowedRange;

    //public float normalizedIndicatorPosition = 0;

    private float randomActualPosition;
    private float barRange;

    void Start()
    {
        topPivotSlider = skillCheckSlider.maxValue;
        bottemPivotSlider = skillCheckSlider.minValue;
        /*Debug.Log("top pivot is: " + topPivotSlider + " and bottem pivot is: " + bottemPivotSlider);*/

        lineIsMoving = true;
        valueMovingUp = true;

        float parentHeight = ((RectTransform)transform).sizeDelta.x;
        barRange = (parentHeight) / 2;                                             //185 / 2
        clampedRange = (parentHeight - targetRangeImage.sizeDelta.y) / 2;     //185 -20 / 2
        allowedRange = clampedRange / barRange;

        randomPosition = Random.Range (-allowedRange, allowedRange);
        //randomPosition = allowedRange;
        // Debug.Log(range);

        randomActualPosition = randomPosition * barRange;
        targetRangeImage.anchoredPosition = new Vector2(randomActualPosition, 0);


    }


    private void Update()
    {
        UpdateTargetRangeVisual();
        SkillCheck();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            lineIsMoving = false;
            if (movingLineValue >= randomLineBorder && movingLineValue <= randomLineBorder + 0.2f)
            {
                Debug.Log("You did it!");
            }
            else
            {
                Debug.Log("You did not do it...");
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {

            lineIsMoving = true;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            randomPosition = Random.Range(-allowedRange, allowedRange);
            //randomPosition = allowedRange;
            // Debug.Log(range);

            randomActualPosition = randomPosition * barRange;
            targetRangeImage.anchoredPosition = new Vector2(randomActualPosition, 0);

            lineIsMoving = true;
        }

        //Debug.Log("The line is at: " + movingLineValue);
        //Debug.Log("The border is in between: " + randomLineBorder + " and: " + (randomLineBorder + 0.2f));
    }

    void SkillCheck()
    {
        if (movingLineValue >= topPivotSlider || movingLineValue <= bottemPivotSlider)
        {
            velocity *= -1f;
        }
        if (lineIsMoving)
        {
            movingLineValue += velocity;
        }

        skillCheckSlider.value = movingLineValue;

    }
    void UpdateTargetRangeVisual()
    {
        float topPosition = randomActualPosition + (targetRangeImage.sizeDelta.y / 2) + 2;
        float bottomPosition = randomActualPosition - (targetRangeImage.sizeDelta.y / 2) - 2;
        
        float sliderPosition = skillCheckSlider.value * barRange;

        Debug.Log(sliderPosition <= topPosition && sliderPosition >= bottomPosition);
    }
}
