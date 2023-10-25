using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    // Reference to the Charging script
    public Charging chargingScript;
    private bool hasPlayed = false;

    Animation animation;
    void Start()
    {
        animation = GetComponent<Animation>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!hasPlayed && chargingScript.charge == chargingScript.maxcharge)
        {
            animation.Play();
            hasPlayed = true;
        }
    }

}
