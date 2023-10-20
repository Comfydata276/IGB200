using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CircuitBreaker : MonoBehaviour, IInteractable
{
    public Charging chargingSystem;
    public GameObject finalMinigame;
    public TextMeshProUGUI messageText;
    public bool textactive = false;
    public GameObject topMinigame;

    void Start()
    {
        messageText.enabled = false;
    }

    public void Interact()
    {
        if (chargingSystem.charge >= 99)
        {
            topMinigame.SetActive(true);
            finalMinigame.SetActive(true);
            messageText.enabled = false;
            textactive = false;
        }
        else
        {
            textactive = !textactive;
            messageText.enabled = textactive;

            if (textactive)
            {
                messageText.text = "You must complete additional objectives before you may attempt this minigame";
            }
        }
    }

    public void Deactivate()
    {
        // Add your deactivation logic here
        // For example, you might want to hide the message text and deactivate the final mini-game:
        messageText.enabled = false;
        textactive = false;
        topMinigame.SetActive(false);
        finalMinigame.SetActive(false);
    }
}
