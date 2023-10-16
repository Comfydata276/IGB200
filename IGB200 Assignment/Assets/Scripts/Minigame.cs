using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour, IInteractable
{
    public GameObject topGame;
    public GameObject game;
    public GameObject world;
    public GameObject interactionText;
    public GameObject UI;
    public GameObject MinigameUI;
    public Charging Charging;

    // Start is called before the first frame update
    void Start()
    {
        topGame.SetActive(false);
        game.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Interact()
    {
        topGame.SetActive(true);
        game.SetActive(true);
        world.SetActive(false);
        interactionText.SetActive(false);
        UI.SetActive(false);
        MinigameUI.SetActive(true);

        //free the cursor
        Cursor.lockState = CursorLockMode.Confined;
        //Show the cursor
        Cursor.visible = true;
    }

    public void Victory()
    {
        topGame.SetActive(false);
        game.SetActive(false);
        world.SetActive(true);
        interactionText.SetActive(true);
        UI.SetActive(true);
        MinigameUI.SetActive(false);

        //Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        //Hide the cursor
        Cursor.visible = false;

        Charging.AddCharge(33);

    }
}
