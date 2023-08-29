using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour, IInteractable
{
    public GameObject game;
    public GameObject world;
    public GameObject interactionText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        game.SetActive(true);
        world.SetActive(false);
        interactionText.SetActive(false);

        //free the cursor
        Cursor.lockState = CursorLockMode.None;
        //Show the cursor
        Cursor.visible = true;


    }
}
