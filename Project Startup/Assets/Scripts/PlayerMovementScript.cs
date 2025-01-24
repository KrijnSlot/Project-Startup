using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float movingSpeed;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Ground;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 movingDirection;

    Rigidbody rb;

    InputEvents inputEvents => InputEvents.instance;

    // Start is called before the first frame update
    void Start()
    {
        inputEvents.walkAction += MyInput;
        inputEvents.walkAction += (Vector2 juan) => { Debug.Log("Walk"+juan); };
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = groundDrag; // grounded is not being used yet so for now this
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput(Vector2 input)
    {
        horizontalInput = input.x;
        verticalInput = input.y;
    }

    private void MovePlayer()
    {
        movingDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(movingDirection.normalized * movingSpeed * 10f, ForceMode.Force);
    }
}   
