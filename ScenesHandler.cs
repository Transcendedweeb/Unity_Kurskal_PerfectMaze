using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesHandler : MonoBehaviour
{
    public void StartNormalMode()
    {
        SceneManager.LoadScene("NormalMode");
    }

    public void StartNightmarelMode()
    {
        SceneManager.LoadScene("NightmareMode");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
