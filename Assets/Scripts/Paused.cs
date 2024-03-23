using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Paused : MonoBehaviour
{
    public GameObject PauseMenuCanvas;
    
    public void ResumeGame()
    {
        PauseMenuCanvas.SetActive(false);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void RestartGame()
    {
        // Reset all the variables
        SceneManager.LoadScene("MainScene");
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
}
