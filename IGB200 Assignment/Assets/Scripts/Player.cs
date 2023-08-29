using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float charge = 0;
    private float maxCharge = 100;
    public GameObject shock;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShortCircuit()
    {
        shock.SetActive(true);
    }
}
