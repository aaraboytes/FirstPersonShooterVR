using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject shootHole;
    [SerializeField] GameObject shootParticle;
    [SerializeField] LayerMask objectiveLayer;
    private void OnCollisionEnter(Collision collision)
    {
        RaycastHit hit;
        Vector3 dir = collision.GetContact(0).point - transform.position;
        if (Physics.Raycast(transform.position, dir, out hit, 0.01f,objectiveLayer))
        {
            GameObject shotHole = Pool.Instance.Recycle(shootHole, Vector3.zero, Quaternion.identity);
            GameObject shotParticle = Pool.Instance.Recycle(shootParticle, Vector3.zero, Quaternion.identity);

            Quaternion shotHoleRot = Quaternion.LookRotation(hit.normal);
            //shotHoleRot = Quaternion.Euler(shotHoleRot.eulerAngles.x, shotHoleRot.eulerAngles.y + 90f, shotHoleRot.z);
            Vector3 shotHolePos = hit.point;
            shotHolePos += dir * 0.01f;

            shotHole.transform.position = shotHolePos;
            shotHole.transform.rotation = shotHoleRot;

            shotParticle.transform.position = shotHolePos;
            shotParticle.transform.rotation = shotHoleRot;

            shotHole.transform.parent = collision.transform;
        }
    }
}
