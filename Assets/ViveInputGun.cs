using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class ViveInputGun : MonoBehaviour
{
    public SteamVR_TrackedObject mTrackedObject = null;
    public SteamVR_Controller.Device mDevice;
    public GameObject weapon = null;
    GameObject ammo = null;
    public bool grabbing = false;
   
    //Weapon
    Transform spawnPoint;
    AmmoType ammoType;


    ParticleSystem shotParticle;
    GameObject collisionParticle;
    int currentBullets = 0;
    int maxBullets = 0;
    int currentAmmo = 0;
    int maxAmmo = 0;
    float cadence = 0;
    float force = 0;
    AudioSource audio;

    private void Awake()
    {
        mTrackedObject = GetComponent<SteamVR_TrackedObject>();
    }
    private void Start()
    {
    }
    void Update()
    {
        mDevice = SteamVR_Controller.Input((int)mTrackedObject.index);
        #region Triggger
        //Down
        if (mDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (weapon != null && currentBullets > 0 && grabbing)
            {
                currentBullets--;
                Shot();
            }else if (weapon!= null && currentBullets == 0 && grabbing)
            {
                Reload();
            }else if (weapon==null && ammo != null)
            {
                PickUpAmmo();
            }
        }
        //Up
        if (mDevice.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            
        }
        Vector2 triggerVal = mDevice.GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger);
        #endregion
        #region Grip (Grab / Drop gun)
        //Down
        if (mDevice.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            if (!grabbing && weapon !=null)
                GrabWeapon();
            else if (grabbing)
                DropWeapon();
        }
        //Up
        if (mDevice.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            
        }
        #endregion
        #region TouchPad
        //Down
        if (mDevice.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            print("TouchpadDown");
        //Up
        if (mDevice.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
            print("TouchpadUp");
        Vector2 touchValue = mDevice.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
        #endregion
    }

    #region Weapon
    void GrabWeapon()
    {
        grabbing = true;
        //Setup shot properties
        Weapon w = weapon.GetComponent<Weapon>();
        SetUpGun(w);
        //Add fixed joint
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.breakForce = 20000;
        joint.breakTorque = 20000;
        joint.connectedBody = weapon.GetComponent<Rigidbody>();
        //Make other rigidbody kinematic 
        weapon.transform.GetChild(0).GetComponent<Collider>().enabled = false;
    }
    void DropWeapon()
    {
        grabbing = false;

        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            //Supposed to have pose.getvelocity() and pose.getangularvelocity()
            weapon.GetComponent<Rigidbody>().velocity = Vector3.zero;
            weapon.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            weapon.GetComponent<Rigidbody>().isKinematic = false;
        }
        weapon.transform.GetChild(0).GetComponent<Collider>().enabled = true;
        weapon = null;
    }
    #endregion
    #region Shot
    void SetUpGun(Weapon w)
    {
        ammoType = w.ammoType;
        maxBullets = w.maxBullets;
        currentBullets = maxBullets;
        maxAmmo = w.maxAmmo;
        cadence = w.cadence;
        force = w.force;
        spawnPoint = w.spawnPoint;
        shotParticle = w.shotParticle;
        collisionParticle = w.collisionParticle;
        audio = weapon.GetComponent<AudioSource>();

        currentAmmo = 100;
    }
    void Shot()
    {
        RaycastHit hit;
        Vector3 dir = spawnPoint.forward;
        shotParticle.Play();
        LevelManager.Instance.IncreaseBulletsShooted();
        Debug.DrawLine(spawnPoint.transform.position, spawnPoint.transform.position + spawnPoint.forward * 1, Color.red);
        audio.Play();
        /*if (Physics.Raycast(spawnPoint.position, dir, out hit))
        {
            print("Collision with wall");
            GameObject hole = Pool.Instance.Recycle(bulletHole, Vector3.zero, Quaternion.identity);
            GameObject shotPart = Pool.Instance.Recycle(collisionParticle, Vector3.zero, Quaternion.identity);
            
            Quaternion shotHoleRot = Quaternion.LookRotation(hit.normal);
            Vector3 shotHolePos = hit.point - dir * 0.01f;

            hole.transform.position = shotHolePos;
            hole.transform.rotation = shotHoleRot;

            shotPart.transform.position = shotHolePos;
            shotPart.transform.rotation = shotHoleRot;

            hole.transform.parent = hit.transform;
            if (hit.collider.gameObject.GetComponent<target>())
            {
                hit.collider.gameObject.GetComponent<target>().Hit(hit.point);
            }
            if (hit.collider.gameObject.GetComponent<TargetZone>())
            {
                hit.collider.gameObject.GetComponent<TargetZone>().Hit(hit.point);
            }
        }*/
        GameObject b = Pool.Instance.Recycle(LevelManager.Instance.FindKindOfBullet(ammoType),
                                            spawnPoint.transform.position,
                                            Quaternion.LookRotation(spawnPoint.forward));
        b.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * force);
    }
    void Reload()
    {
        if (currentAmmo > 0)
        {
            currentBullets += maxBullets;
            currentAmmo--;
        }
    }
    void PickUpAmmo()
    {
        if (currentAmmo < maxAmmo && ammo.GetComponent<Ammo>().ammo == ammoType)
        {
            currentAmmo++;
            Destroy(ammo);
            ammo = null;
        }
    }
    #endregion
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Weapon") && weapon == null && other.transform.parent.GetComponent<Rigidbody>())
        {
            weapon = other.transform.parent.gameObject;
        }else if (other.CompareTag("Ammo"))
        {
            ammo = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!grabbing)
            weapon = null;
        if(ammo!=null)ammo = null;
    }
}
