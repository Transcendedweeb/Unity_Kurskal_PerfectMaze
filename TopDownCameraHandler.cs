using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCameraHandler : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float zoomSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 20f;

    Camera cam;

    void MoveCamera()
    {
        float moveX = Input.GetAxis("Horizontal"); // mapped A & D
        float moveZ = Input.GetAxis("Vertical"); // mapped W & S

        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime; // creates vector based on user input
        this.transform.Translate(move, Space.World);  // move the camera
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // get scrollwheel input
        float newZoom = cam.orthographicSize - scroll * zoomSpeed; // set the new zoom

        newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom); // clamp the zoom
        cam.orthographicSize = newZoom;
    }

    void Start()
    {
        cam = this.gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        MoveCamera();
        HandleZoom();
    }
}
