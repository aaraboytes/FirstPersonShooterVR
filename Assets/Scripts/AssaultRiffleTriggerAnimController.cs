using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRiffleTriggerAnimController : MonoBehaviour
{ 
    [Header("Animation")]
    public Animator anime;
    public GameObject manopla;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            manopla.GetComponent<Animator>().Play("Trigg1");
        }

        else  if (Input.GetKeyUp("space"))
            {
            manopla.GetComponent<Animator>().Play("Trigg2");
        }

      
    }
}
