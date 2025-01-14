using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbstractFishingScript : MonoBehaviour
{

    public float castRange = 5;


    public virtual void castRod()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }

}
