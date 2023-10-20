using UnityEngine;

public class StoryDialogue : MonoBehaviour, IInteractable
{
    public GameObject textbox;
    public bool textactive = false;
    public GameObject UI;

    public void Interact()
    {
        if (!textactive)
        {
            textbox.SetActive(true);
            textactive = true;
            UI.SetActive(false);
        }
        else
        {
            textbox.SetActive(false);
            textactive = false;
            UI.SetActive(true);
        }
    }

    public void Deactivate()
    {
        // Deactivation logic, similar to turning off the text box and enabling the UI
        textbox.SetActive(false);
        textactive = false;
        UI.SetActive(true);
    }
}
