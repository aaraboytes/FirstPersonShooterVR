using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class ViveInputGun : MonoBehaviour
{
    public SteamVR_TrackedObject mTrackedObject = null;
    public SteamVR_Controller.Device mDevice;
    [SerializeField]
    GameObject weapon = null;
    bool grabbing = false;
   
    //weapon
    Transform spawnPoint;
    AmmoType type;
    public GameObject bulletHole;
    ParticleSystem shotParticle;
    GameObject collisionParticle;
    int maxBullets = 0;
    float cadence = 0;
    float force = 0;

    private void Awake()
    {
        mTrackedObject = GetComponent<SteamVR_TrackedObject>();
    }
    void Update()
    {
        mDevice = SteamVR_Controller.Input((int)mTrackedObject.index);
        #region Triggger
        //Down
        if (mDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (weapon != null)
                Shot();
        }
        //Up
        if (mDevice.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            
        }
        Vector2 triggerVal = mDevice.GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger);
        #endregion
        #region Grip
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
        //Make other rigidbody kinematic^vzzzz 
        weapon.GetComponent
        
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
        }

        weapon = null;
    }
    #endregion
    #region Shot
    void SetUpGun(Weapon w)
    {
        type = w.ammoType;
        maxBullets = w.maxBullets;
        cadence = w.cadence;
        force = w.force;
        spawnPoint = w.spawnPoint;
        shotParticle = w.shotParticle;
    }
    void Shot()
    {
        RaycastHit hit;
        Vector3 dir = spawnPoint.forward;
        shotParticle.Play();
        LevelManager.Instance.IncreaseBulletsShooted();
        Debug.DrawLine(spawnPoint.transform.position, spawnPoint.transform.position + spawnPoint.forward * 1, Color.red);
        if (Physics.Raycast(spawnPoint.position, dir, out hit))
        {
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
        }
    }
    void Reload()
    {

    }
    #endregion
    private void OnTriggerStay(Collider other)
    {
        print(other.name);
        if(other.CompareTag("Weapon") && weapon == null && other.transform.parent.GetComponent<Rigidbody>())
        {
            weapon = other.transform.parent.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!grabbing)
            weapon = null;
    }
}
