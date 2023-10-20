using System.Collections.Generic;
using System.Linq;
using UnityEngine;

interface IInteractable
{
    void Interact();
    void Deactivate();
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;
    public GameObject interactionText;

    private List<IInteractable> interactablesInRange = new List<IInteractable>();

    void Update()
    {
        CheckForInteraction();
    }

    void CheckForInteraction()
    {
        // Clear the list for this frame
        interactablesInRange.Clear();

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, InteractRange))
        {
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                // Interactable object is in range, enable interaction text and add to list
                interactionText.SetActive(true);
                interactablesInRange.Add(interactObj);
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
            if (interactablesInRange.Count > 0)
            {
                foreach (var interactable in interactablesInRange)
                {
                    interactable.Interact();
                }
            }
            else
            {
                // If 'E' is pressed and no interactables are in range, deactivate all
                IInteractable[] allInteractables = FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>().ToArray();
                foreach (var interactable in allInteractables)
                {
                    interactable.Deactivate();
                }
            }
        }
    }
}
