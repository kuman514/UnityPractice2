using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private AudioSource weaponSound;

    // reference
    public Transform shootPoint;
    public Text ammoUI;
    public AudioClip fireSE;
    public AudioClip reloadSE;

    // Start is called before the first frame update
    void Start()
    {
        fireTimer = 0.0f;
        reloading = reloadTime;
        currentAmmo = ammoPerMag;
        anim = GetComponent<Animator>();
        weaponSound = GetComponent<AudioSource>();
        SetAmmoText();
    }

    // Update is called once per frame
    void Update()
    {
        // processing =============================================
        if(Input.GetButton("Fire1"))
        {
            // don't fire until fully reloaded
            if(currentAmmo > 0)
            {
                Fire();

                // auto reload
                if(currentAmmo <= 0)
                {
                    Reload();
                }
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
        // end of processing ======================================

        // UI drawing =============================================
        SetAmmoText();
        // end of UI drawing ======================================
    }

    void Fire()
    {
        if(fireTimer >= fireRate && reloading >= reloadTime)
        {
            currentAmmo--;

            RaycastHit hit;
            if(Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, shootRange))
            {
                // if hit
                Debug.Log(weaponName + " hit / Remaining ammo: " + currentAmmo + "/" + ammoPerMag);
            }
            else
            {
                // if miss
                Debug.Log(weaponName + " miss / Remaining ammo: " + currentAmmo + "/" + ammoPerMag);
            }

            anim.CrossFadeInFixedTime("Fire", fireRate);
            weaponSound.PlayOneShot(fireSE);

            fireTimer = 0.0f;
        }
    }

    void Reload()
    {
        if(reloading >= reloadTime)
        {
            weaponSound.PlayOneShot(reloadSE);
            anim.CrossFadeInFixedTime("Reload", reloadTime);
            reloading = 0.0f;
        }
    }

    void Run()
    {
        anim.CrossFadeInFixedTime("Run", reloadTime);
    }

    void SetAmmoText()
    {
        ammoUI.text = currentAmmo + " / " + totalAmmo;
    }
}
