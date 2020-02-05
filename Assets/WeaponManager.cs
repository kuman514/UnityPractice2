using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // Weapon Specification
    public string weaponName;
    public int ammoPerMag;
    public int currentAmmo;
    public int totalAmmo;
    public float shootRange;
    public float fireRate;
    public float reloadTime;

    // uneditable
    private float fireTimer;
    private float reloading;
    private Animator anim;
    private AudioSource fireSound;

    // reference
    public Transform shootPoint;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = ammoPerMag;
        anim = GetComponent<Animator>();
        fireSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1"))
        {
            // don't fire until fully reloaded
            if(currentAmmo > 0 && reloading >= reloadTime)
            {
                Fire();
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            if (currentAmmo < ammoPerMag)
            {
                Reload();
            }
        }

        if (fireTimer < fireRate)
        {
            // fire timer charging
            fireTimer += Time.deltaTime;
        }

        if (reloading < reloadTime)
        {
            // reloading timer charging
            reloading += Time.deltaTime;

            // fully reloaded
            if (reloading >= reloadTime)
            {
                totalAmmo -= ammoPerMag;
                currentAmmo = ammoPerMag;
            }
        }
    }

    void Fire()
    {
        if(fireTimer >= fireRate)
        {
            currentAmmo--;

            RaycastHit hit;
            if(Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, shootRange))
            {
                // if hit
                print(weaponName + " hit / Remaining ammo: " + currentAmmo + "/" + ammoPerMag);
            }
            else
            {
                // if miss
                print(weaponName + " miss / Remaining ammo: " + currentAmmo + "/" + ammoPerMag);
            }

            anim.CrossFadeInFixedTime("Fire", fireRate);
            fireSound.Play();
            
            fireTimer = 0.0f;
        }
    }

    void Reload()
    {
        anim.CrossFadeInFixedTime("Reload", reloadTime);
        reloading = 0.0f;
    }

    void Run()
    {
        
    }
}
