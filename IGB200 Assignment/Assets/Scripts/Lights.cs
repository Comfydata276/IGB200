using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
    public GameObject[] lights; // Array to hold references to the light GameObjects
    public float delayTime = 1.0f; // Time delay between activating each light

    private bool hasPlayed = false; // Variable to track if audio and lights have been activated

    // This method is called when an object collides with the trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Check if the audio and lights have not been activated yet
            if (!hasPlayed)
            {
                // Start the coroutine to activate lights and audio
                StartCoroutine(ActivateLights());

                // Set hasPlayed to true so this won't happen again
                hasPlayed = true;
            }
        }
    }

    // Coroutine to handle the timed activation of lights and audio
    private IEnumerator ActivateLights()
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(true); // Activate the light

            // Get the AudioSource component and play it if it exists
            AudioSource audioSource = light.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }

            yield return new WaitForSeconds(delayTime); // Wait for 'delayTime' seconds
        }
    }
}
