using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWeapons
{
    public GameObject model;
    public AmmoType type;
    public Transform spawnPoint;
    public ParticleSystem shotFire;
}
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    Transform camera;
    [Header("Shot")]
    [SerializeField] bool armed = false;
    [SerializeField] GameObject model;
    [SerializeField] AmmoType ammoType = AmmoType.gun;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float cadence = 0.2f;
    [SerializeField] float force = 500f;
    int ammo = 0;
    int maxBullets = 10;
    [SerializeField]int bullets = 0;
    float time = 0;
    [Header("Bullet")]
    [SerializeField] GameObject shootHole;
    [SerializeField] GameObject shootParticle;
    [SerializeField] ParticleSystem shotFire;
    [SerializeField] List<PlayerWeapons> playerWeapons = new List<PlayerWeapons>();

    Vector3 move;
    CharacterController player;

    private void Start()
    {
        if (!armed && model!=null) model.SetActive(false);
        player = GetComponent<CharacterController>();
        camera = Camera.main.transform;
        Cursor.visible = false;
    }
    void Update()
    {
        #region Move
        float yStored = player.velocity.y;
        move = Input.GetAxis("Horizontal") * camera.transform.right + Input.GetAxis("Vertical") * camera.transform.forward;
        move *= speed;
        move.y = yStored;
        if(!player.isGrounded)
            AddGravity();
        player.Move(move * Time.deltaTime);
        #endregion
        #region Shot
        time += Time.deltaTime;
        if (Input.GetMouseButton(0) && time >= cadence && bullets>0)
        {
            bullets--;
            Shot();
            time = 0;
        }
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
        #endregion
    }
    #region Movement
    void AddGravity()
    {
        if (!player.isGrounded)
            move.y += Physics.gravity.y;
    }
    #endregion
    #region Shot
    public void SetUpGun(Weapon weapon)
    {
        maxBullets = weapon.maxBullets;
        ammoType = weapon.ammoType;
        cadence = weapon.cadence;
        force = weapon.force;
        foreach(PlayerWeapons w in playerWeapons)
        {
            if(w.type == ammoType)
            {
                if (model != null) model.SetActive(false);
                model = w.model;
                spawnPoint = w.spawnPoint;
                shotFire = w.shotFire;
                model.SetActive(true);
                break;
            }
        }
        armed = true;
    }
    void Shot()
    {
        RaycastHit hit;
        Vector3 dir = camera.forward;
        shotFire.Play();
        LevelManager.Instance.IncreaseBulletsShooted();
        Debug.DrawLine(camera.transform.position, camera.transform.position + camera.forward * 1, Color.red);
        if (Physics.Raycast(camera.position, dir, out hit))
        {
            GameObject shotHole = Pool.Instance.Recycle(shootHole, Vector3.zero, Quaternion.identity);
            GameObject shotParticle = Pool.Instance.Recycle(shootParticle, Vector3.zero, Quaternion.identity);

            Quaternion shotHoleRot = Quaternion.LookRotation(hit.normal);
            Vector3 shotHolePos = hit.point - dir * 0.01f;

            shotHole.transform.position = shotHolePos;
            shotHole.transform.rotation = shotHoleRot;

            shotParticle.transform.position = shotHolePos;
            shotParticle.transform.rotation = shotHoleRot;

            shotHole.transform.parent = hit.transform;
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
    bool IncreaseAmmo(AmmoType type)
    {
        if (ammoType == type)
        {
            ammo++;
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
    #region ColliderTrigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo"))
        {
            if(IncreaseAmmo(other.GetComponent<Ammo>().ammo))
                Destroy(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            if (armed)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    SetUpGun(other.GetComponent<Weapon>());
                }
            }
            else
            {
                SetUpGun(other.GetComponent<Weapon>());
            }
        }
    }
    #endregion
}
