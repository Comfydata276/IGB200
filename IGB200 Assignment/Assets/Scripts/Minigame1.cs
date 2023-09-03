using UnityEngine;
using TMPro;  // Namespace for TextMeshPro
using UnityEngine.UI;  // Namespace for Unity's UI
using System.Collections.Generic;

public class Minigame1 : MonoBehaviour
{
    public TextMeshProUGUI powerStatusText;  // Reference to the TextMeshProUGUI element
    public RawImage loseImage;  // Reference to the Image UI element for the loss screen
    private bool isPowerOn = true;  // Power status, true by default

    // To store original positions of the circuit components
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        loseImage.enabled = false;  // Hide the lose image initially
        UpdatePowerStatus();  // Initialize the power status text

        // Record original positions of all objects with tag "Circuit"
        foreach (GameObject circuitObject in GameObject.FindGameObjectsWithTag("Circuit"))
        {
            originalPositions[circuitObject] = circuitObject.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Your game update logic here
    }

    // Called when player collides with any game object
    void OnTriggerEnter(Collider other)
    {
        // Check if power is on and player collides with circuit components
        if (isPowerOn && other.gameObject.CompareTag("Circuit"))
        {
            LoseGame();
        }
    }

    // This method will be linked to the UI Button's OnClick event
    public void TogglePowerStatus()
    {
        isPowerOn = !isPowerOn;  // Toggle the power status
        UpdatePowerStatus();  // Update the text element
    }

    // Update the TextMeshPro element based on power status
    private void UpdatePowerStatus()
    {
        if (isPowerOn)
        {
            powerStatusText.text = "Power On";
            powerStatusText.color = Color.green;
        }
        else
        {
            powerStatusText.text = "Power Off";
            powerStatusText.color = Color.red;
        }
    }

    // Method to handle losing the game
    private void LoseGame()
    {
        loseImage.enabled = true;  // Show the lose image
        ResetGame();  // Reset the game objects
    }

    // Method to reset game objects to their original state
    private void ResetGame()
    {
        foreach (var entry in originalPositions)
        {
            entry.Key.transform.position = entry.Value;
        }
    }
}
