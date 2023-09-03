using System;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCableSystem : MonoBehaviour
{
    public GameObject cablePrefab;
    public GameObject faultyCablePrefab;
    public Camera mainCamera;
    public LayerMask clickableLayer;

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

    private bool cableMode;

    private List<Tuple<GameObject, GameObject, LineRenderer>> connections = new List<Tuple<GameObject, GameObject, LineRenderer>>();

    void Start()
    {
        foreach (CableConnection connection in initialCables)
        {
            CreateCable(connection.startPoint, connection.endPoint, connection.isFaulty);
        }
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
                    Debug.Log("Cable is touching object: " + hit.collider.gameObject.name);
                }
            }
        }

        if (cableMode)
        {
            if (Input.GetMouseButtonDown(0))
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
        }
    }

    private void CreateCable(GameObject start, GameObject end, bool isFaulty)
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

        connections.Add(new Tuple<GameObject, GameObject, LineRenderer>(start, end, lineRenderer));

        Debug.Log("Connected: " + start.name + " and " + end.name + (isFaulty ? " (Faulty)" : " (Normal)"));
    }

    private bool IsAlreadyConnected(GameObject start, GameObject end)
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
