using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;

    [HideInInspector] public bool holdingGun;
    [HideInInspector] public bool holdingRod;

    public GameObject gunObj;
    public GameObject rodObj;

    [HideInInspector] public GameObject heldObj;

    public float throwForce = 500f;
    public float pickUpRange = 5f;
    private float rotationSensitivity = 3f;
    private Rigidbody heldObjRb;
    private bool canDrop = true;
    private int LayerNumber;
    [SerializeField] TMP_Text grabIndicator;

    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer");

    }
    void Update()
    {
        GrabIndicator();

        if (Input.GetKeyDown(KeyCode.E))
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
                        PickUpObject(hit.transform.gameObject);
                    }

                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Q) && canDrop == true)
        {
            StopClipping(); 
            DropObject();
        }

        if (heldObj != null)
        {
            MoveObject(); 
            RotateObject();
            if (Input.GetKeyDown(KeyCode.T) && canDrop == true)
            {
                StopClipping();
                ThrowObject();
            }
        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            heldObj = pickUpObj;
            heldObjRb = pickUpObj.GetComponent<Rigidbody>();
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform;
            heldObj.layer = LayerNumber;
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);

            if (heldObj.Equals(gunObj)) { holdingGun = true; }
            else if (heldObj.Equals(rodObj)) { holdingRod = true; }
        }
    }
    void DropObject()
    {
        //re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObj = null;
    }
    void MoveObject()
    {
        heldObj.transform.position = holdPos.transform.position;
        heldObj.transform.rotation = holdPos.transform.rotation;
    }
    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))//hold R key to rotate.
        {
            canDrop = false; //make sure throwing can't occur during rotating

            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
            heldObj.transform.Rotate(Vector3.down, XaxisRotation);
            heldObj.transform.Rotate(Vector3.right, YaxisRotation);
        }
        else
        {
            canDrop = true;
        }
    }
    void ThrowObject()
    {
        //same as drop function, but add force to object before undefining it
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
    }
    void StopClipping()
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); 

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        if (hits.Length > 1)
        {
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
        //disabling the gun and/or the rod when throwing something.
        holdingGun = false;
        holdingRod = false;
    }

    void GrabIndicator()
    {
        if (heldObj == null)
        {

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
            {
                //make sure pickup tag is attached
                if (hit.transform.gameObject.tag == "canPickUp")
                {
                    grabIndicator.enabled = true;

                }
                else grabIndicator.enabled = false;

            }
        }
        else if (heldObj != null)
        {
            grabIndicator.enabled = false;
        }
    }
}