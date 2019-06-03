using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bulletHole;
    public GameObject collisionParticle;
    private void OnCollisionEnter(Collision other)
    {
        GameObject hole = Pool.Instance.Recycle(bulletHole, Vector3.zero, Quaternion.identity);
        GameObject shotPart = Pool.Instance.Recycle(collisionParticle, Vector3.zero, Quaternion.identity);

        Quaternion shotHoleRot = Quaternion.FromToRotation(transform.forward,other.contacts[0].normal);
        Vector3 shotHolePos = other.contacts[0].point;

        hole.transform.position = shotHolePos;
        hole.transform.rotation = shotHoleRot;

        shotPart.transform.position = shotHolePos;
        shotPart.transform.rotation = shotHoleRot;

        hole.transform.parent = other.transform;

        print(other.gameObject.name);
        if (other.gameObject.GetComponent<target>())
        {
            other.gameObject.GetComponent<target>().Hit(transform.position);
            print("Component target");
        }
        if (other.gameObject.GetComponent<TargetZone>())
        {
            other.gameObject.GetComponent<TargetZone>().Hit(transform.position);
            print("Component targetZone");
        }
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
