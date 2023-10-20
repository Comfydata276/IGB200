using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
    public GameObject[] lights; // Array to hold references to the light GameObjects
    public float delayTime = 1.0f; // Time delay between activating each light

    // This method is called when an object collides with the trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Start the coroutine to activate lights
            StartCoroutine(ActivateLights());
        }
    }

    // Coroutine to handle the timed activation of lights
    private IEnumerator ActivateLights()
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(true); // Activate the light
            yield return new WaitForSeconds(delayTime); // Wait for 'delayTime' seconds
        }
    }
}
