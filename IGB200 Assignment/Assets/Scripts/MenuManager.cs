using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public int gameStartScene;
    public int mainMenuScene;

    // Update is called once per frame
    public void StartGame()
    {
        SceneManager.LoadScene(gameStartScene);
    }
    public void QuitGame()
    {
        Application.Quit();
        Console.WriteLine("Quit");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
