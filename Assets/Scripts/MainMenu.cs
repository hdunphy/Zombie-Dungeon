using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPressedPlay()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnPressedOptions()
    {

    }

    public void OnPressedQuit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
