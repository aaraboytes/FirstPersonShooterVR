using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVR : MonoBehaviour
{
    [Header("Shot")]
    [SerializeField] bool armed = false;
    [SerializeField] GameObject weapon;
    [SerializeField] AmmoType ammoType = AmmoType.none;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float cadence = 0.2f;
    [SerializeField] float force = 500f;
    int ammo = 0;
    int maxBullets = 10;
    int bullets = 0;
    float time;

    [Header("Bullet")]
    [SerializeField] GameObject shotHole;
    [SerializeField] GameObject shotParticle;
    [SerializeField] ParticleSystem shotFire;

    private void Update()
    {
        time += Time.deltaTime;
        if(Input.GetKeyDown(0) && time>=cadence && bullets > 0)
        {
            bullets--;
            Shot();
            time = 0;
        }
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
        if (Input.GetKeyDown(KeyCode.E))
        {
            weapon.transform.parent = null;
        }
    }
    #region Shot
    void Shot()
    {
        RaycastHit hit;
        Vector3 dir = spawnPoint.forward;
        shotFire.Play();
        LevelManager.Instance.IncreaseBulletsShooted();
        Debug.DrawLine(spawnPoint.transform.position, spawnPoint.transform.position + spawnPoint.forward * 1, Color.red);
        if (Physics.Raycast(spawnPoint.position, dir, out hit))
        {
            GameObject sh = Pool.Instance.Recycle(shotHole, Vector3.zero, Quaternion.identity);
            GameObject sp = Pool.Instance.Recycle(shotParticle, Vector3.zero, Quaternion.identity);

            Quaternion shotHoleRot = Quaternion.LookRotation(hit.normal);
            Vector3 shotHolePos = hit.point - dir * 0.01f;

            sh.transform.position = shotHolePos;
            sh.transform.rotation = shotHoleRot;

            sp.transform.position = shotHolePos;
            sp.transform.rotation = shotHoleRot;

            sh.transform.parent = hit.transform;
            if (hit.collider.gameObject.GetComponent<target>())
            {
                hit.collider.gameObject.GetComponent<target>().Hit(hit.point);
            }
            if (hit.collider.gameObject.GetComponent<TargetZone>())
            {
                hit.collider.gameObject.GetComponent<TargetZone>().Hit(hit.point);
            }
        }
    }

    void Reload()
    {
        if (ammo > 0)
        {
            ammo--;
            bullets = maxBullets;
        }
    }
    #endregion
    public void SetWeapon(GameObject _weapon)
    {
        weapon = _weapon;
        armed = true;
        SetUpGun(weapon.GetComponent<Weapon>());
    }
    public void SetUpGun(Weapon weapon)
    {
        maxBullets = weapon.maxBullets;
        ammoType = weapon.ammoType;
        cadence = weapon.cadence;
        force = weapon.force;
        spawnPoint = weapon.spawnPoint;
        armed = true;
    }
}
