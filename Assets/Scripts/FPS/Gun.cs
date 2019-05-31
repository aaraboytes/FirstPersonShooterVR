using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject bullet;
    [SerializeField] float force;
    [SerializeField] float cadence;
    float time = 0;
    void Update()
    {
        Vector3 movement = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        if (Input.GetKey(KeyCode.Q))
            movement.y = 1;
        else if (Input.GetKey(KeyCode.E))
            movement.y = -1;
        transform.Translate(movement * 2 * Time.deltaTime);
        time += Time.deltaTime;
        if (Input.GetMouseButton(0) && time>= cadence)
        {
            GameObject b = Pool.Instance.Recycle(bullet, Vector3.zero, Quaternion.identity);
            b.transform.position = spawnPoint.position;
            b.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * force);
            time = 0;
        }
    }
}
