using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int maxBullets;
    public int maxAmmo;
    public AmmoType ammoType;
    public float cadence;
    public float force;
    public Transform spawnPoint;
    public ParticleSystem shotParticle;
    public GameObject collisionParticle;
}
