using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Victory : MonoBehaviour
{
    public GameObject VictoryUI;
    public GameObject targetGameObject;

    // This method is called when an object collides with the trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            ActivateVictory();
        }
    }

    private void ActivateVictory()
    {
        // Free the cursor
        Cursor.lockState = CursorLockMode.None;
        // Show the cursor
        Cursor.visible = true;

        // Get a reference to the script component attached to the game object
        FirstPersonController script = targetGameObject.GetComponent<FirstPersonController>();
        // Disable the script component
        script.enabled = false;
        // Activate the Victory UI
        VictoryUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // You can leave this empty if you have no other logic to update every frame
    }
}
