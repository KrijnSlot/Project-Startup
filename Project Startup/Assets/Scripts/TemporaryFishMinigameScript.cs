using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryFishMinigameScript : MonoBehaviour
{
    float movingLineValue, randomLineBorder;
    bool lineIsMoving, valueMovingUp;
    // Start is called before the first frame update
    void Start()
    {
        movingLineValue = -1.01f;
        lineIsMoving = true;
        valueMovingUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        SkillCheck();
        Debug.Log("the line is at: " + movingLineValue);
        Debug.Log("the border is inbetween: "+ randomLineBorder + " and :" + randomLineBorder + 0.2f );
    }

    void SkillCheck()
    {
        if (movingLineValue >= 1f)
        {
            valueMovingUp = false;
        }
        else if (movingLineValue <= -1f)
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            lineIsMoving = false;
            if (movingLineValue >= randomLineBorder && movingLineValue <= randomLineBorder + 0.2f)
            {
                Debug.Log("you did it!");
            }
            else
            {
                Debug.Log("you did not do it...");
            }
        }
    }
}
