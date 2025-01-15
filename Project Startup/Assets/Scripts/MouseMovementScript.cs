using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MouseMovementScript : MonoBehaviour
{
    public Transform playerOrientation;

    public float mouseSensitivity = 400f;

    public float xRotation = 0f;
    public float yRotation = 0f;

    public float topClamp = -89f;
    public float botClamp = 89f;

    void Start()
    {
        // makes the mouse invisible and the position locked to the middel of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // gets the mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // rotates around the x axis to look up and down
        xRotation -= mouseY;

        // Clamps rotation
        xRotation = Mathf.Clamp(xRotation, topClamp, botClamp);

        // rotates around the y axis to look right and left
        yRotation += mouseX;

        // applies the rotations to transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
