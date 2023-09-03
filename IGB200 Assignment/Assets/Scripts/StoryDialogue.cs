using System.Collections;
using System.Collections.Generic;
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
}
