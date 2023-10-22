using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickerlight : MonoBehaviour
{
    private Light lightSource;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2.0f;
    public float flickerRate = 0.1f;

    private void Start()
    {
        lightSource = GetComponent<Light>();
        if (lightSource == null)
        {
            Debug.LogError("No Light component found on this GameObject. Please attach this script to a GameObject with a Light component.");
            return;
        }

        InvokeRepeating("Flicker", 0, flickerRate);
    }

    private void Flicker()
    {
        if (lightSource.enabled)
        {
            lightSource.intensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}