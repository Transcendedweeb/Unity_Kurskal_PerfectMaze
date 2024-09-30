/*
    nightmare mode uses 3 MazeBuilder scripts instead of 1
    because of this the OnDisable of the MazeBuilder script needs to change
    these changes are handled here
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NightmareModeController : MonoBehaviour
{
    public GameObject[] generators; // set the 3 maze gameobjects

    public Button stopButton;
    public Button playButton;

    public MazeButtonHandlers mazeButtonHandler;

    int counter = 0;
    [HideInInspector] public bool playActive = false;

    public void ResetAllMazes()
    {
        playActive = false;
        stopButton.interactable = false;
        counter = 0;
        foreach (GameObject generator in generators) // loop through the 3 mazes
        {
            foreach (Transform child in generator.transform) // loop through the children of the current parent
            {
                Destroy(child.gameObject); // remove the childern to reset the maze
            }
            generator.SetActive(false);
            generator.GetComponent<MazeBuilder>().enabled = true; // this is turned of the moment a maze is generated, so it needs to be enabled again
        }
    }

    public void RegenerateMazes() // reset the all mazes before starting a new generation
    {
        ResetAllMazes();
        mazeButtonHandler.GenerateMazeButtonPress();
    }

    public void ActivateNextGenerator() // is called to handle the next maze to be generated
    {
        counter++; // maze index 
        switch (counter)
        {
            case 3: // all 3 mazes have been generated
                stopButton.interactable = false;
                playButton.interactable = true;
                counter--; // set index to the last generated maze

                // disable builder of last maze so when it is active again during play mode it doesn't generate a new maze
                generators[counter].GetComponent<MazeBuilder>().enabled = false;
                break;
            default:
                // set the height and width of the last generated maze
                generators[counter].GetComponent<MazeBuilder>().height = generators[counter-1].GetComponent<MazeBuilder>().height;
                generators[counter].GetComponent<MazeBuilder>().width = generators[counter-1].GetComponent<MazeBuilder>().width;

                generators[counter-1].GetComponent<MazeBuilder>().enabled = false; 
                generators[counter].SetActive(true); // start generating the next maze
                generators[counter-1].SetActive(false);
                break;
        }
    }

    public void StartMapSwitches()
    {
        playActive = true;
        StartCoroutine(SwitchingMaps());
    }

    IEnumerator SwitchingMaps()
    {
        int activeMaze = counter;
        while (playActive) // as long as the player is in the maze
        {
            yield return new WaitForSeconds(7f);
            if (playActive) // an extra check to make sure no problems arise in the 7 wait seconds
            {
                generators[activeMaze].SetActive(false);

                // sets the next maze active
                if (activeMaze == 2) activeMaze = 0;
                else activeMaze++;
            }
            generators[activeMaze].SetActive(true);
        }
    }
}
