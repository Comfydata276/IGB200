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
    public Charging charging;
    public CircuitBreaker circuitBreaker;

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

    // Union-Find data structure
    Dictionary<GameObject, GameObject> parent = new Dictionary<GameObject, GameObject>();

    // Find operation for Union-Find
    GameObject Find(GameObject u)
    {
        if (u == parent[u])
        {
            return u;
        }
        else
        {
            parent[u] = Find(parent[u]); // Path compression
            return parent[u];
        }
    }

    // Union operation for Union-Find
    void Union(GameObject u, GameObject v)
    {
        u = Find(u);
        v = Find(v);
        if (u != v)
        {
            parent[v] = u; // Merge sets
        }
    }

    public void ToggleCableMode()
    {
        cableMode = !cableMode;
        Debug.Log("Cable mode: " + cableMode);

        if (cableMode)
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
        DeleteAllCables();

        // Initialize Union-Find
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Circuit"))
        {
            parent[go] = go; // Each node is its own parent initially
        }

        foreach (CableConnection connection in initialCables)
        {
            CreateCable(connection.startPoint, connection.endPoint, connection.isFaulty);
        }
        cableModeText.text = "Cable Mode Off";
        cableModeText.color = Color.red;
    }

    public void DeleteAllCables()
    {
        // Iterate over each cable connection and destroy its associated GameObject
        foreach (var tuple in connections)
        {
            Destroy(tuple.Item3.gameObject);  // Destroy the LineRenderer's GameObject
        }
        // Clear the connections list
        connections.Clear();
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
            if (Input.GetMouseButtonDown(0)) // Left click
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer))
                {
                    // Check if the object is tagged as 'hazard'
                    if (hit.collider.gameObject.CompareTag("hazard"))
                    {
                        Debug.Log("Cannot attach cable to hazard: " + hit.collider.gameObject.name);
                        //game over if player touches a hazard when power is on
                        if (minigame1.isPowerOn)
                        {
                            minigame1.LoseGame("Power Hazard!");
                        }
                        return;
                    }

                    Debug.Log("Object detected: " + hit.collider.gameObject.name);

                    if (firstObject == null)
                    {
                        firstObject = hit.collider.gameObject;
                    }
                    else
                    {
                        //game over if player connnects a wire when power is on
                        if (minigame1.isPowerOn)
                        {
                            minigame1.LoseGame("Power Hazard!");
                        }
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
                        //game over if player disconnnects a wire when power is on
                        if (minigame1.isPowerOn)
                        {
                            minigame1.LoseGame("Power Hazard!");
                        }
                        secondObject = hit.collider.gameObject;
                        RemoveSpecificCable(firstObject, secondObject);
                        firstObject = null;
                        secondObject = null;
                    }
                }
            }
        }
    }


    public void CheckForHazard()
    {
        // First, check for incomplete circuit
        CheckForIncompleteCircuit();

        bool hazardDetected = false;
        HashSet<GameObject> visited = new HashSet<GameObject>();
        Dictionary<GameObject, int> connectionCount = new Dictionary<GameObject, int>();

        // Populate connectionCount with all GameObjects tagged as "Circuit"
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Circuit"))
        {
            connectionCount[go] = 0;
        }

        // Count the number of connections for each object in the circuit
        foreach (var tuple in connections)
        {
            if (tuple.Item1.CompareTag("Circuit")) connectionCount[tuple.Item1]++;
            if (tuple.Item2.CompareTag("Circuit")) connectionCount[tuple.Item2]++;
        }

        // Check if all components have exactly two connections
        foreach (var count in connectionCount.Values)
        {
            if (count < 2)
            {
                Debug.Log("Incomplete circuit! All components must have exactly two connections.");
                minigame1.LoseGame("Incomplete circuit!");
                return;
            }
        }


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
        if (!hazardDetected && charging.charge >= 99)
        {
            // Assuming minigameInstance is a reference to an instance of the Minigame class
            circuitBreaker.Victory();
        } else if (!hazardDetected )
        {
            minigame.Victory();
        }
    }


    private void DFS(GameObject current, HashSet<GameObject> visited)
    {
        if (current == null || visited.Contains(current)) return;

        visited.Add(current);

        foreach (var tuple in connections)
        {
            if (tuple.Item1 == current)
            {
                DFS(tuple.Item2, visited);
            }
            else if (tuple.Item2 == current)
            {
                DFS(tuple.Item1, visited);
            }
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

    public void CheckForIncompleteCircuit()
    {
        // 1. Use DFS to traverse the circuit and identify all visited components
        HashSet<GameObject> visited = new HashSet<GameObject>();
        if (connections.Count > 0) // Ensure there's at least one connection to start DFS
        {
            DFS(connections[0].Item1, visited);
        }

        // 2. Check if all components have at least two connections
        Dictionary<GameObject, int> connectionCount = new Dictionary<GameObject, int>();
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Circuit"))
        {
            connectionCount[go] = 0;
        }
        foreach (var tuple in connections)
        {
            if (tuple.Item1.CompareTag("Circuit")) connectionCount[tuple.Item1]++;
            if (tuple.Item2.CompareTag("Circuit")) connectionCount[tuple.Item2]++;
        }
        foreach (var kvp in connectionCount)
        {
            if (kvp.Value < 2)
            {
                Debug.Log($"Incomplete circuit! Component '{kvp.Key.name}' has only {kvp.Value} connection(s). It should have at least 2 connections.");
            }
        }

        // 3. Compare the visited set with all components
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Circuit"))
        {
            if (!visited.Contains(go))
            {
                Debug.Log($"Component '{go.name}' is not connected to the main circuit.");
            }
        }
    }


}
