//using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FishMinigameScript : MonoBehaviour
{
    /// <summary>
    /// 
    /// WE NEED TO MAKE THE UI SHOW WHEN YOU START FISHING SINCE ITS NOT IMPLEMENTED YET
    /// 
    /// </summary>


    public static FishMinigameScript instance;
    [Header("GameObjects")]
    //[SerializeField] public GameObject skillCheckUI;
    [SerializeField] private Slider skillCheckSlider; // Reference to the UI Slider
    [SerializeField] private RectTransform targetRangeImage;
    [Header("Speed")]
    [SerializeField] float velocity = 1.2f;
    [Header("Progress Bar")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private float progressSliderVelocity = -2.5f;

    float randomPosition, allowedRange, clampedRange, parentHeight;
    private float movingLineValue, topPivotSlider, bottemPivotSlider, randomActualPosition, barRange, topPosition, bottomPosition, sliderPosition;
    private bool lineIsMoving, pressCooldown;

    InputEvents inputEvents => InputEvents.instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        inputEvents.skillCheckAction += SkillCheckActions;
        topPivotSlider = skillCheckSlider.maxValue;
        bottemPivotSlider = skillCheckSlider.minValue;
        /*Debug.Log("top pivot is: " + topPivotSlider + " and bottem pivot is: " + bottemPivotSlider);*/

        lineIsMoving = true;
        pressCooldown = true;

        parentHeight = ((RectTransform)transform).sizeDelta.x;
        barRange = (parentHeight) / 2;                                             //185 / 2
        clampedRange = (parentHeight - targetRangeImage.sizeDelta.y) / 2;     //185 -20 / 2
        allowedRange = clampedRange / barRange;

        RandomizeTargetPos();


    }


    private void Update()
    {
        UpdateTargetRangeVisual();
        SkillCheck();

        if (Input.GetKeyDown(KeyCode.I)) // these 2 inputs are for testing and will not be used for the final game
            lineIsMoving = true;

        if (Input.GetKeyDown(KeyCode.U))
        {
            RandomizeTargetPos();
            //randomPosition = allowedRange;
            // Debug.Log(range);

            lineIsMoving = true;
        }
    }

    void SkillCheck()
    {
        if (this.gameObject == !false)
        {

            if (movingLineValue >= topPivotSlider || movingLineValue <= bottemPivotSlider) // makes the slider move up and down
                velocity *= -1f;

            if (lineIsMoving)
                movingLineValue += velocity * Time.deltaTime; // frame independant

            if (progressSlider.value >= progressSlider.maxValue)
            {
                Debug.Log("You caught a fish!"); // turn off the background and catch the fish here
                progressSlider.value = 0f;
                // gotta make up a way to remove the entire thing
                this.gameObject.SetActive(false); 
            }
        }

        skillCheckSlider.value = movingLineValue;
        progressSlider.value += progressSliderVelocity * Time.deltaTime; // frame independent
    }

    private void SkillCheckActions()
    {
        if (!pressCooldown)
            return;

        pressCooldown = false;
        RandomizeTargetPos();

        if (sliderPosition <= topPosition && sliderPosition >= bottomPosition)
            progressSlider.value += 10f;
        else
            progressSlider.value -= 5f;
        StartCoroutine(CooldownSkillcheck(0.2f));
    }

    private void RandomizeTargetPos()
    {
        randomPosition = Random.Range(-allowedRange, allowedRange);
        //randomPosition = allowedRange;
        // Debug.Log(range);

        randomActualPosition = randomPosition * barRange;
        targetRangeImage.anchoredPosition = new Vector2(randomActualPosition, 0);
    }

    private IEnumerator CooldownSkillcheck(float delay)
    {
        yield return new WaitForSeconds(delay);
        pressCooldown = true;
    }

    void UpdateTargetRangeVisual()
    {
        topPosition = randomActualPosition + (targetRangeImage.sizeDelta.y / 2) + 2;
        bottomPosition = randomActualPosition - (targetRangeImage.sizeDelta.y / 2) - 2;

        sliderPosition = skillCheckSlider.value * barRange;

        Debug.Log(sliderPosition <= topPosition && sliderPosition >= bottomPosition);
    }
}
