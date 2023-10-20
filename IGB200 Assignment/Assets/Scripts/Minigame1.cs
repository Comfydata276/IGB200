using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class Minigame1 : MonoBehaviour
{
    public TextMeshProUGUI powerStatusText;
    public RawImage loseImage;
    public Button resetButton;
    public TextMeshProUGUI loseText;
    public bool isPowerOn = true;
    public DynamicCableSystem dynamicCableSystem;

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
        loseText.text = $"A critical error was detected with your circuit! \nDetailed: {reason}\nClick Reset to try again.";
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

        foreach (var tuple in dynamicCableSystem.connections)
        {
            if (tuple.Item3 != null)
            {
                Destroy(tuple.Item3.gameObject);
            }
        }
        dynamicCableSystem.connections.Clear();

        foreach (DynamicCableSystem.CableConnection connection in dynamicCableSystem.initialCables)
        {
            dynamicCableSystem.CreateCable(connection.startPoint, connection.endPoint, connection.isFaulty);
        }

        // Set power to on by default
        isPowerOn = true;
        UpdatePowerStatus();

        // Toggle off the cable mode
        dynamicCableSystem.cableMode = true;
        dynamicCableSystem.ToggleCableMode();
    }
}
