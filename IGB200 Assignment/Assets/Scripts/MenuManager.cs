using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class MenuManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    public GameObject targetGameObject;

    public int gameStartScene;
    public int mainMenuScene;

    public bool mouseOff = true;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Get a reference to the script component attached to the game object
            FirstPersonController script = targetGameObject.GetComponent<FirstPersonController>();
            
            if (mouseOff)
            {
                //free the cursor
                Cursor.lockState = CursorLockMode.None;
                //Show the cursor
                Cursor.visible = true;

                // Disable the script component
                script.enabled = false;
                mouseOff = false;
            }
            else
            {
                //Lock the cursor
                Cursor.lockState = CursorLockMode.Locked;
                // Hide the cursor
                Cursor.visible = false;

                // Disable the script component
                script.enabled = true;
                mouseOff = true;
            }
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(gameStartScene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0.0f;
        isPaused = true;

        //free the cursor
        Cursor.lockState = CursorLockMode.None;
        //Show the cursor
        Cursor.visible = true;

        // Get a reference to the script component attached to the game object
        FirstPersonController script = targetGameObject.GetComponent<FirstPersonController>();
        // Disable the script component
        script.enabled = false;

        //crosshair.SetActive(false);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;

        //Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Hide the cursor
        Cursor.visible = false;

        // Get a reference to the script component attached to the game object
        FirstPersonController script = targetGameObject.GetComponent<FirstPersonController>();
        // Disable the script component
        script.enabled = true;

        //crosshair.SetActive(true);
    }
}
