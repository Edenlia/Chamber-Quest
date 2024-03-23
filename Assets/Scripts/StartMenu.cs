using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Reset all the variables
        SceneManager.LoadScene("MainScene");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
