using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DynamicCableSystem : MonoBehaviour
{
    // Existing fields
    public GameObject cablePrefab;
    public GameObject faultyCablePrefab;
    public Camera mainCamera;
    public LayerMask clickableLayer;
    public Minigame1 minigame1;
    public Minigame minigame;

    [Serializable]
    public class CableConnection
    {
        public GameObject startPoint;
        public GameObject endPoint;
        public bool isFaulty;
    }

    public CableConnection[] initialCables;

    private GameObject firstObject;
    private GameObject secondObject;
    public bool cableMode;
    public TextMeshProUGUI cableModeText;

    // Added a bool to the tuple to store if the cable is faulty
    public List<Tuple<GameObject, GameObject, LineRenderer, bool>> connections = new List<Tuple<GameObject, GameObject, LineRenderer, bool>>();

    public void ToggleCableMode()
    {
        cableMode = !cableMode;
        Debug.Log("Cable mode: " + cableMode);

        if (cableMode) // Replace with your actual condition
        {
            cableModeText.text = "Cable Mode On";
            cableModeText.color = Color.green;
        }
        else
        {
            cableModeText.text = "Cable Mode Off";
            cableModeText.color = Color.red;
        }
    }

    void Start()
    {
        foreach (CableConnection connection in initialCables)
        {
            CreateCable(connection.startPoint, connection.endPoint, connection.isFaulty);
        }
        cableModeText.text = "Cable Mode Off";  // Assume initially off
        cableModeText.color = Color.red;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cableMode = !cableMode;
            Debug.Log("Cable mode: " + cableMode);
        }

        foreach (var tuple in connections)
        {
            tuple.Item3.SetPosition(0, tuple.Item1.transform.position);
            tuple.Item3.SetPosition(1, tuple.Item2.transform.position);

            // Raycasting to detect objects the cable is touching
            RaycastHit hit;
            Vector3 direction = tuple.Item2.transform.position - tuple.Item1.transform.position;
            float distance = Vector3.Distance(tuple.Item1.transform.position, tuple.Item2.transform.position);
            if (Physics.Raycast(tuple.Item1.transform.position, direction, out hit, distance))
            {
                if (hit.collider.gameObject != tuple.Item1 && hit.collider.gameObject != tuple.Item2)
                {
                    //Debug.Log("Cable is touching object: " + hit.collider.gameObject.name);
                }
            }
        }

        if (cableMode)
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && minigame1.isPoweron == false)
            {
                if (Input.GetMouseButtonDown(0)) // Left click
                {
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer))
                    {
                        Debug.Log("Object detected: " + hit.collider.gameObject.name);

                        if (firstObject == null)
                        {
                            firstObject = hit.collider.gameObject;
                        }
                        else
                        {
                            secondObject = hit.collider.gameObject;
                            CreateCable(firstObject, secondObject, false);

                            firstObject = null;
                            secondObject = null;
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(1))  // Right click
                {
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer))
                    {
                        Debug.Log("Right-clicked on: " + hit.collider.gameObject.name);

                        if (firstObject == null)
                        {
                            firstObject = hit.collider.gameObject;
                        }
                        else
                        {
                            secondObject = hit.collider.gameObject;
                            RemoveSpecificCable(firstObject, secondObject);
                            firstObject = null;
                            secondObject = null;
                        }
                    }
                }
            }   else
            {
                Debug.Log("Power detected! Triggering lose condition.");
                minigame1.LoseGame("The power is still on! Please dont forget to disable it!");  // Trigger the lose condition
                hazardDetected = true;  // Set flag to true
                break;  // No need to check further
            }
        }
    }
    public void CheckForHazard()
    {
        bool hazardDetected = false;  // flag to track if hazard is found

        foreach (var tuple in connections)
        {
            RaycastHit hit;
            Vector3 direction = tuple.Item2.transform.position - tuple.Item1.transform.position;
            float distance = Vector3.Distance(tuple.Item1.transform.position, tuple.Item2.transform.position);

            if (Physics.Raycast(tuple.Item1.transform.position, direction, out hit, distance))
            {
                if (hit.collider.gameObject.CompareTag("hazard"))
                {
                    Debug.Log("Hazard detected! Triggering lose condition.");
                    minigame1.LoseGame("Hazard Detected in Circuit!");  // Trigger the lose condition
                    hazardDetected = true;  // Set flag to true
                    break;  // No need to check further
                }
            }

            if (tuple.Item4)  // If cable is faulty
            {
                Debug.Log("Faulty cable detected! Triggering lose condition.");
                minigame1.LoseGame("Fault Cable Detected");
                hazardDetected = true;  // Set flag to true
                break;  // No need to check further
            }
        }

        // If no hazard is detected, call the Victory method
        if (!hazardDetected)
        {
            // Assuming minigameInstance is a reference to an instance of the Minigame class
            minigame.Victory();
        }
    }

    public void CreateCable(GameObject start, GameObject end, bool isFaulty)
    {
        if (IsAlreadyConnected(start, end))
        {
            Debug.Log("Objects already connected: " + start.name + " and " + end.name);
            return;
        }

        GameObject cable = Instantiate(isFaulty ? faultyCablePrefab : cablePrefab);
        LineRenderer lineRenderer = cable.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, start.transform.position);
        lineRenderer.SetPosition(1, end.transform.position);

        connections.Add(new Tuple<GameObject, GameObject, LineRenderer, bool>(start, end, lineRenderer, isFaulty));

        Debug.Log("Connected: " + start.name + " and " + end.name + (isFaulty ? " (Faulty)" : " (Normal)"));
    }

    public void RemoveSpecificCable(GameObject start, GameObject end)
    {
        Tuple<GameObject, GameObject, LineRenderer, bool> cableToRemove = null;

        foreach (var tuple in connections)
        {
            if ((tuple.Item1 == start && tuple.Item2 == end) || (tuple.Item1 == end && tuple.Item2 == start))
            {
                cableToRemove = tuple;
                break;
            }
        }

        if (cableToRemove != null)
        {
            connections.Remove(cableToRemove);
            Destroy(cableToRemove.Item3.gameObject);  // Remove the LineRenderer's GameObject
        }
    }

    public bool IsAlreadyConnected(GameObject start, GameObject end)
    {
        foreach (var tuple in connections)
        {
            if ((tuple.Item1 == start && tuple.Item2 == end) || (tuple.Item1 == end && tuple.Item2 == start))
            {
                return true;
            }
        }
        return false;
    }
}
