//using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TemporaryFishMinigameScript : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private Slider skillCheckSlider; // Reference to the UI Slider
    [SerializeField] private Slider progressBar;
    private float progress = 1;
    [SerializeField] private float progressStep = 0.3f;
    [SerializeField] private RectTransform targetRangeImage;
    [Header("Speed")]
    [SerializeField] float velocity = 0.01f;
    
    float randomPosition, allowedRange, clampedRange, parentHeight;
    private float movingLineValue, topPivotSlider, bottemPivotSlider, randomActualPosition, barRange;
    private bool lineIsMoving;

    void Start()
    {
        topPivotSlider = skillCheckSlider.maxValue;
        bottemPivotSlider = skillCheckSlider.minValue;
        /*Debug.Log("top pivot is: " + topPivotSlider + " and bottem pivot is: " + bottemPivotSlider);*/

        lineIsMoving = true;

        parentHeight = ((RectTransform)transform).sizeDelta.x;
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
            /*if (movingLineValue >= randomLineBorder && movingLineValue <= randomLineBorder + 0.2f)
            {
                if (progress != -1)
                {
                    Debug.Log("You did it!");
                    randomLineBorder = UnityEngine.Random.Range(-1f, 0.8f);
                    lineIsMoving = true;
                    progress -= progressStep;
                } 
                else
                {
                    SkillText.text = "you won";
                }

            }
            else
            {
                Debug.Log("You did not do it...");
            }*/
        }
        if (Input.GetKeyDown(KeyCode.I))
        {

            lineIsMoving = true;
            progress = 1;
            SkillText.text = "press space";
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
            movingLineValue += velocity * Time.deltaTime;
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

    void UpdateProgress() 
    { 
        progressBar.value = progress;
        if (progress == -1)
        {
            progress = 0;
        }

    }
}
