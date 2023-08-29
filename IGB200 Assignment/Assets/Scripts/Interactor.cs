using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{

    public Transform InteractorSource;
    public float InteractRange;
    //public Scoring charge;
    public GameObject menu;
    public GameObject scoremenu;
    public int count = 0;

    public GameObject interactionText;

    void Update()
    {
        CheckForInteraction();
    }

    void CheckForInteraction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, InteractRange))
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                // Interactable object is in range, enable interaction text
                interactionText.SetActive(true);
            }
            else
            {
                // Interactable object is out of range, disable interaction text
                interactionText.SetActive(false);
            }
        }
        else
        {
            // No interactable object in range, disable interaction text
            interactionText.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
            {
                // Interactable object is in range and E key is pressed, interact with object
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
}
