using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemporaryFishMinigameScript : MonoBehaviour
{
    [SerializeField] private Slider skillCheckSlider; // Reference to the UI Slider
    [SerializeField] private RectTransform targetRangeImage;
    private float movingLineValue, randomLineBorder, topPivotSlider, bottemPivotSlider;
    private bool lineIsMoving, valueMovingUp;

    void Start()
    {
        topPivotSlider = skillCheckSlider.maxValue;
        bottemPivotSlider = skillCheckSlider.minValue;
        /*Debug.Log("top pivot is: " + topPivotSlider + " and bottem pivot is: " + bottemPivotSlider);*/

        lineIsMoving = true;
        valueMovingUp = true;

        randomLineBorder = UnityEngine.Random.Range(-1f, 0.8f);
    }

    void Update()
    {
        UpdateTargetRangeVisual();
        SkillCheck();
        /*Debug.Log("The line is at: " + movingLineValue);
        Debug.Log("The border is in between: " + randomLineBorder + " and: " + (randomLineBorder + 0.2f));*/
    }

    void SkillCheck()
    {
        if (movingLineValue >= topPivotSlider)
        {
            valueMovingUp = false;
        }
        else if (movingLineValue <= bottemPivotSlider)
        {
            valueMovingUp = true;
        }

        if (valueMovingUp && lineIsMoving)
        {
            movingLineValue += 0.01f;
        }
        else if (!valueMovingUp && lineIsMoving)
        {
            movingLineValue -= 0.01f;
        }

        skillCheckSlider.value = movingLineValue;

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
            randomLineBorder = UnityEngine.Random.Range(-1f, 0.8f);
            lineIsMoving = true;
        }
    }
    void UpdateTargetRangeVisual()
    {
        // Map randomLineBorder (-1 to 1) to slider normalized range (0 to 1)
        float normalizedStart = (randomLineBorder + 1f) / 2f;
        float normalizedEnd = (randomLineBorder + 0.2f + 1f) / 2f;

        // Update the target range visual
        targetRangeImage.anchorMin = new Vector2(normalizedStart, 0.5f);
        targetRangeImage.anchorMax = new Vector2(normalizedEnd, 0.5f);
        targetRangeImage.localScale = new Vector2(0.56f, 1);
        targetRangeImage.anchoredPosition = Vector2.zero;
    }
}