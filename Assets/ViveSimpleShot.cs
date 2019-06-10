using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveSimpleShot : MonoBehaviour
{
    public SteamVR_TrackedObject mTrackedObject = null;
    public SteamVR_Controller.Device mDevice;

    public Weapon weapon;
    //Gun params
    AmmoType ammoType;
    Transform spawnPoint;
    ParticleSystem shotParticle;
    int currentBullets = 0;
    int maxBullets = 0;
    int currentAmmo = 0;
    int maxAmmo = 0;
    float cadence = 0;
    float force = 0;

    //Audio params
    [Header("Audio")]
    public AudioClip shotSound;
    public AudioClip emptySound,reloadSound;
    AudioSource audio;

    float timer = 0;

    private void Awake()
    {
        mTrackedObject = GetComponent<SteamVR_TrackedObject>();
    }
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        mDevice = SteamVR_Controller.Input((int)mTrackedObject.index);
        #region Trigger
        //Down
        timer += Time.deltaTime;
        if (mDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if(currentBullets > 0 && timer>cadence)
            {
                Shot();
            }
            else
            {
                audio.PlayOneShot(emptySound);
            }
            timer = 0;
        }
        #endregion
        #region TouchPad
        //Down
        if (mDevice.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Reload();
        }
        #endregion
    }
    void SetUpWeapon()
    {
        ammoType = weapon.ammoType;
        maxBullets = weapon.maxBullets;
        currentBullets = maxBullets;
        maxAmmo = weapon.maxAmmo;
        cadence = weapon.cadence;
        force = weapon.force;
        spawnPoint = weapon.spawnPoint;
        shotParticle = weapon.shotParticle;
        audio = weapon.GetComponent<AudioSource>();

        currentAmmo = 100;
    }
    void Shot()
    {
        Vector3 dir = spawnPoint.forward;
        shotParticle.Stop();
        shotParticle.Play();
        LevelManager.Instance.IncreaseBulletsShooted();
        Debug.DrawLine(spawnPoint.transform.position, spawnPoint.transform.position + spawnPoint.forward * 1, Color.red);
        audio.PlayOneShot(shotSound);
        GameObject b = Pool.Instance.Recycle(LevelManager.Instance.FindKindOfBullet(ammoType),
                                            spawnPoint.transform.position,
                                            Quaternion.LookRotation(spawnPoint.forward));
        b.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * force, ForceMode.Impulse);
    }
    void Reload()
    {
        if (currentAmmo > 0)
        {
            currentBullets = maxAmmo;
            currentAmmo--;
        }
    }
}
