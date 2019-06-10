using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparar : MonoBehaviour
{
    public Animator anime;
    public GameObject manopla;
   // private SteamVR_TrackedController device;

    void Start()
    {
      //  device = GetComponent<SteamVR_TrackedController>();
       //device.TriggerClicked += trigger;
    }

    void Update()
    { 
        if (Input.GetKey("space"))
        {
            manopla.GetComponent<Animator>().Play("trigger3");
        }
        else
        {
        manopla.GetComponent<Animator>().Play("New State");
        }
    }

   /* void trigger (object sender, ClickedEventArgs e)
    {
        Debug.Log("Disparo");
        SteamVR_Controller.Input((int)device.index).TriggerHapticPulse(500);
    }*/
}