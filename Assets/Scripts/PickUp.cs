using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PickUp : MonoBehaviour
{
    public Material triggerMat;
    Material originalMat;
    public Transform spawnPoint;
    public Weapon weaponProperties;

    private void Start()
    {
        originalMat = transform.GetChild(0).GetComponent<Renderer>().material;
    }
    private void OnTriggerStay(Collider other)
    {
        transform.GetChild(0).GetComponent<Renderer>().material = triggerMat;
        if (Input.GetKeyDown(KeyCode.P))
        {
            transform.parent = other.transform;
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.LookRotation(other.transform.forward);
            other.GetComponent<PlayerVR>().SetWeapon(gameObject);
            originalMat = transform.GetChild(0).GetComponent<Renderer>().material;
            this.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        transform.parent.GetComponent<Renderer>().material = originalMat;
    }
}
