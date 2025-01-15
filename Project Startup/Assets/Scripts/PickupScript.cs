using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public float pickUpRange = 5.0f;

    public bool gun;
    public bool rod;

    public GameObject gunObj;
    public GameObject rodObj;

    [HideInInspector] public GameObject heldObj;

    private void Start()
    {
        gunObj.SetActive(false);
        rodObj.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) //change E to whichever key you want to press to pick up
        {
            if (heldObj == null)
            {

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    //make sure pickup tag is attached
                    if (hit.transform.gameObject.tag == "canPickUp")
                    {
                        //pass in object hit into the PickUpObject function
                        pickUpObj(hit.transform.gameObject);
                    }

                }
            }
        }
    }

    void pickUpObj(GameObject holding)
    {
        if(holding.transform.gameObject.tag == "gun")
        {
            rodObj.SetActive (false);
            gunObj.SetActive (true);
            gun = true;
        }
        else if (holding.transform.gameObject.tag == "rod")
        {
            gunObj.SetActive(false);
            rodObj.SetActive(true);
            rod = true;
        }
        else
        {
            gunObj.SetActive(false);
            rodObj.SetActive(false);
            gun = false;
            rod = false;

        }
    }
}
