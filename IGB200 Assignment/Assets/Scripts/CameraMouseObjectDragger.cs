using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  

public class DragObject : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    private Quaternion originalRotation;  // Store the original rotation

    public TextMeshProUGUI displayText;  // Reference to the TextMeshProUGUI component

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

        // Store the original rotation
        originalRotation = transform.rotation;

        // Log the name of the object to the console
        Debug.Log("Object clicked: " + gameObject.name);

        // Update the TextMeshProUGUI object
        if (displayText != null)
        {
            displayText.text = gameObject.name;
        }
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x, y)
        Vector3 mousePoint = Input.mousePosition;

        // Z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        Vector3 newPosition = GetMouseAsWorldPoint() + mOffset;

        // Lock the y-coordinate to its current value
        newPosition.y = transform.position.y;

        // Set the new position
        transform.position = newPosition;

        // Lock the rotation to its original value
        transform.rotation = originalRotation;
    }
}
