using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class message : MonoBehaviour
{
    public GameObject textbox;
    public bool textactive = false;
    public GameObject UI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isActiveAndEnabled & !textactive)
        {
            if (!textactive)
            {
                textbox.SetActive(true);
                textactive = true;
                UI.SetActive(false);
            }
            else
            {
                textbox.SetActive(false);
                textactive = false;
                UI.SetActive(true);
            }
        }
    }

    public void messageOpen()
    {
        if (!textactive)
        {
            textbox.SetActive(true);
            textactive = true;
            UI.SetActive(false);
        }
        else
        {
            textbox.SetActive(false);
            textactive = false;
            UI.SetActive(true);
        }
    }
}
