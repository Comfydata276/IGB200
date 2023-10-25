using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CircuitBreaker : MonoBehaviour, IInteractable
{
    public Charging chargingSystem;
    public GameObject finalMinigame;
    public TextMeshProUGUI messageText;
    public bool textactive = false;
    public GameObject topMinigame;
    public GameObject World;
    public GameObject launcher;
    public GameObject interactionText;
    public GameObject UI;
    public GameObject MinigameUI;
    public Charging Charging;
    public GameObject VictoryCube;

    void Start()
    {
        messageText.enabled = false;
    }

    public void Interact()
    {
        if (chargingSystem.charge >= 99)
        {
            //topMinigame.SetActive(true);
            finalMinigame.SetActive(true);
            messageText.enabled = false;
            textactive = false;
            World.SetActive(false);
            launcher.SetActive(false);
            interactionText.SetActive(false);
            UI.SetActive(false);
            MinigameUI.SetActive(true);

            // Free the cursor
            Cursor.lockState = CursorLockMode.Confined;
            // Show the cursor
            Cursor.visible = true;
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
        messageText.enabled = false;
        textactive = false;
        //topMinigame.SetActive(false);
        finalMinigame.SetActive(false);
    }

    public void Victory()
    {
        finalMinigame.SetActive(false);
        World.SetActive(true);
        interactionText.SetActive(true);
        UI.SetActive(true);
        MinigameUI.SetActive(false);

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Hide the cursor
        Cursor.visible = false;

        Charging.AddCharge(1);
        VictoryCube.SetActive(true);
    }
}
