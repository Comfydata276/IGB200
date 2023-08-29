using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Charging : MonoBehaviour
{
    public TextMeshProUGUI ChargeText;
    public int charge = 0;
    public int maxcharge;
    
    void Start()
    {
        charge = 0;
    }

    public void AddCharge(int newCharge)
    {
        charge += newCharge;
    }

    public void UpdateCharge()
    {
        ChargeText.text = "Charge: " + charge + "%";
    }

    void Update()
    {
        UpdateCharge();

        if(charge == maxcharge)
        {

        }
        else if(charge > 74)
        {

        }
        else if(charge > 49)
        {

        }
        else if(charge > 24)
        {

        }
        else 
        {

        }
    }
}
