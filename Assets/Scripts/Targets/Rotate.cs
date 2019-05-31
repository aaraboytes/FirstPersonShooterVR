using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed;
    public Vector3 rotAxis = new Vector3(0,1,0);
    Rigidbody body;
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        Invoke("InitializeRotator", 0.1f);
    }
    void InitializeRotator()
    {
        body.AddTorque(Vector3.up * 2);
    }
    public void RotateInAxis(float intensity)
    {
        body.AddRelativeTorque(rotAxis * intensity * speed);
    }
}
