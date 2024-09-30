using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeButtonHandlers : MonoBehaviour
{
    public GameObject player;
    public GameObject topDownCamera;
    public GameObject mazeBuilderObject;
    public GameObject openCanvas;
    public GameObject closedCanvas;
    public InputFieldHandler inputFieldHeight;
    public InputFieldHandler inputFieldWidth;
    public Button generateButton;
    public Button stopButton;
    public Button playButton;
    public Toggle toggle;

    MazeBuilder mazeBuilder;
    bool isToggleTrue = true;

    void Start()
    {
        mazeBuilder = mazeBuilderObject.GetComponent<MazeBuilder>();
    }

    void Update()
    {
        // checks if the toggle is pressed or not
        if (toggle.isOn == isToggleTrue) return; // when the flag and toggle is the same return

        if (toggle.isOn) // toggle is pressed to true
        {
            isToggleTrue = toggle.isOn;
            Vector3 currentEulerAngles = topDownCamera.transform.rotation.eulerAngles;
            topDownCamera.transform.rotation = Quaternion.Euler(90f, currentEulerAngles.y, currentEulerAngles.z); // rotate camera to the maze
        }
        else // togle is pressed to false 
        {
            isToggleTrue = toggle.isOn;
            Vector3 currentEulerAngles = topDownCamera.transform.rotation.eulerAngles;
            topDownCamera.transform.rotation = Quaternion.Euler(-90f, currentEulerAngles.y, currentEulerAngles.z); // rotate camera away from the maze
        }
    }

    public void OpenCanvas()
    {
        openCanvas.SetActive(true);
        closedCanvas.SetActive(false);
    }

    public void CloseCanvas()
    {
        openCanvas.SetActive(false);
        closedCanvas.SetActive(true);
    }

    public void GenerateMazeButtonPress()
    {
        if (mazeBuilderObject.activeSelf) mazeBuilderObject.SetActive(false); // checks if a maze is already build and disables it

        stopButton.interactable = true;
        playButton.interactable = false;
        int height = inputFieldHeight.inputValue; // gets the user input
        int width = inputFieldWidth.inputValue;

        mazeBuilder.height = height; // sets the user input
        mazeBuilder.width = width;
        mazeBuilderObject.SetActive(true); // enable the gameobject to start generating a maze
    }

    public void StopMazeGeneration() // force stop
    {
        stopButton.interactable = false;
        playButton.interactable = false;
        mazeBuilderObject.SetActive(false);
    }

    public void PlayButtonPress()
    {
        player.SetActive(true);
        topDownCamera.SetActive(false);
    }
}
