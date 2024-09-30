using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public GameObject topDownSetup;

    CharacterController controller;
    float xRotation = 0f;

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal"); // mapped A & D
        float moveZ = Input.GetAxis("Vertical"); // mapped W & S

        Vector3 move = this.transform.right * moveX + transform.forward * moveZ; // creates a vector based on user input 

        move.y = 0f;

        controller.Move(move * moveSpeed * Time.deltaTime); // moves playe based on the vector
    }

    void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; // gets horizontal mouse movement
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; // gets vertical mouse movement

        this.transform.Rotate(Vector3.up * mouseX); // rotates player on x-axis based on mouse movement

        xRotation -= mouseY; // update vertical rotation
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // clamps x-rotation to stop user of breaking their necks

        this.transform.localRotation = Quaternion.Euler(xRotation, transform.localEulerAngles.y, 0f); // update all rotations
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        // disables cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();

        // checks if Q is pressed to quit the play mode
        if (Input.GetKeyDown(KeyCode.Q))
        {
            topDownSetup.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            this.gameObject.SetActive(false);
        }
    }
}
