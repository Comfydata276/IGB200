using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minigame : MonoBehaviour, IInteractable
{
    public GameObject game;
    public GameObject world;
    public GameObject interactionText;
    public GameObject UI;
    public GameObject MinigameUI;
    public Charging Charging;
    public Minigame1 minigame1;
    public GameObject Launcher;

    // Start is called before the first frame update
    void Start()
    {
        game.SetActive(false);
        minigame1.ResetGame();  // Reset the game state
    }


    // Update is called once per frame
    void Update()
    {
    }

    public void Interact()
    {
        game.SetActive(true);
        interactionText.SetActive(false);
        UI.SetActive(false);
        MinigameUI.SetActive(true);
        Launcher.SetActive(false);
        world.SetActive(false);

        // Free the cursor
        Cursor.lockState = CursorLockMode.Confined;
        // Show the cursor
        Cursor.visible = true;
    }

    public void Deactivate()
    {
        game.SetActive(false);
        world.SetActive(true);
        interactionText.SetActive(true);
        UI.SetActive(true);
        MinigameUI.SetActive(false);

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Hide the cursor
        Cursor.visible = false;
    }

    public void Victory()
    {
        game.SetActive(false);
        world.SetActive(true);
        interactionText.SetActive(true);
        UI.SetActive(true);
        MinigameUI.SetActive(false);

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Hide the cursor
        Cursor.visible = false;

        Charging.AddCharge(33);

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(1);
        }
    }
}
