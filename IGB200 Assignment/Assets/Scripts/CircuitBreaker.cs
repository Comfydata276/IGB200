using UnityEngine;
using UnityEngine.UI;  // For Text UI
using TMPro;  // For TextMeshPro

public class CircuitBreaker : MonoBehaviour, IInteractable
{
    public Charging chargingSystem;  // Reference to the Charging script
    public GameObject finalMinigame;  // Reference to the final minigame
    public TextMeshProUGUI messageText;  // Reference to TextMeshProUGUI to display messages
    public bool textactive = false;  // Boolean to toggle message text

    // Start is called before the first frame update
    void Start()
    {
        // Optionally initialize UI elements here
        messageText.enabled = false;
    }

    public void Interact()
    {
        if (chargingSystem.charge >= 99)
        {
            // Launch the final minigame
            finalMinigame.SetActive(true);

            // Reset the message and textactive state
            messageText.enabled = false;
            textactive = false;
        }
        else
        {
            // Toggle the message display based on textactive state
            textactive = !textactive;
            messageText.enabled = textactive;

            if (textactive)
            {
                messageText.text = "You must complete additional objectives before you may attempt this minigame";
            }
        }
    }
}
