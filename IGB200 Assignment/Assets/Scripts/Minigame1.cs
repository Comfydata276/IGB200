using UnityEngine;
using TMPro;  // Namespace for TextMeshPro
using UnityEngine.UI;  // Namespace for Unity's UI
using System.Collections.Generic;

public class Minigame1 : MonoBehaviour
{
    public TextMeshProUGUI powerStatusText;  // Reference to the TextMeshProUGUI element
    public RawImage loseImage;  // Reference to the Image UI element for the loss screen
    public Button resetButton;  // Reference to the reset button
    public TextMeshProUGUI loseText;  // Text that shows the error message
    public bool isPowerOn = true;  // Power status, true by default
    public DynamicCableSystem dynamicCableSystem;


    // To store original positions of the circuit components
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();

    void Start()
    {
        loseImage.enabled = false;
        loseText.enabled = false;
        resetButton.gameObject.SetActive(false);
        UpdatePowerStatus();

        foreach (GameObject circuitObject in GameObject.FindGameObjectsWithTag("Circuit"))
        {
            originalPositions[circuitObject] = circuitObject.transform.position;
        }
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (isPowerOn && other.gameObject.CompareTag("Circuit"))
        {
            LoseGame("You touched a live circuit!");
        }
    }

    public void TogglePowerStatus()
    {
        isPowerOn = !isPowerOn;
        UpdatePowerStatus();
    }

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

    public void LoseGame(string reason)
    {
        loseImage.enabled = true;
        loseText.enabled = true;
        resetButton.gameObject.SetActive(true);
        loseText.text = $"A critical error was detecked with your circuit! \nDetailed: {reason}\nClick Reset to try again.";
    }

    public void ResetGame()
    {
        loseImage.enabled = false;
        loseText.enabled = false;
        resetButton.gameObject.SetActive(false);

        foreach (var entry in originalPositions)
        {
            entry.Key.transform.position = entry.Value;
        }
        // Destroy all current cables and clear the connections list
        foreach (var tuple in dynamicCableSystem.connections)
        {
            if (tuple.Item3 != null) // The LineRenderer component of the tuple
            {
                Destroy(tuple.Item3.gameObject);
            }
        }
        dynamicCableSystem.connections.Clear();

        // Recreate the initial set of cables
        foreach (DynamicCableSystem.CableConnection connection in dynamicCableSystem.initialCables)
        {
            dynamicCableSystem.CreateCable(connection.startPoint, connection.endPoint, connection.isFaulty);
        }
    }
}
