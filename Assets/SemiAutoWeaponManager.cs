using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SemiAutoWeaponManager : MonoBehaviour
{
    // Weapon Specification
    public string weaponName;
    public int ammoPerMag;
    public int currentAmmo;
    public int totalAmmo;
    public float shootRange;
    public float fireRate;
    public float reloadTime;
    public float maxScatter;
    public float scatterIncreasePerRound;
    public float scatterReduction;
    public float damagePerRound;
    public float knockbackPerRound;

    // uneditable
    private float fireTimer;
    private float reloading;
    private Animator anim;
    private Vector3 originalPosition;
    private bool isAiming;
    private bool isRunning;
    private float fov;
    private float curScatter;

    // reference
    public Transform shootPoint;
    public Text ammoUI;
    public AudioSource weaponSound;
    public AudioClip fireSE;
    public AudioClip reloadSE;
    public GameObject hitHolePrefab;
    public Camera cam;
    public Vector3 recoilKickback;
    public Vector3 aimPosition;
    public Transform casingExit;
    public GameObject bulletCasing;

    // Start is called before the first frame update
    void Start()
    {
        fireTimer = 0.0f;
        reloading = reloadTime;
        currentAmmo = ammoPerMag;
        anim = GetComponent<Animator>();
        SetAmmoText();
        originalPosition = transform.localPosition;
        fov = cam.fieldOfView;
        curScatter = 0.0f;
        isRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        // processing =============================================
        if(Input.GetButtonDown("Fire1"))
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
                totalAmmo -= (ammoPerMag - currentAmmo);
                currentAmmo = ammoPerMag;
            }
        }

        ReduceScatter();
        AimDownSights();
        Run();
        // end of processing ======================================

        // UI drawing =============================================
        SetAmmoText();
        // end of UI drawing ======================================
    }

    void Fire()
    {
        if(fireTimer >= fireRate && reloading >= reloadTime && !isRunning)
        {
            currentAmmo--;

            RaycastHit hit;
            if(Physics.Raycast(shootPoint.position, shootPoint.transform.forward + Random.onUnitSphere * curScatter, out hit, shootRange))
            {
                // if hit
                Debug.Log(weaponName + " hit / Remaining ammo: " + currentAmmo + "/" + ammoPerMag);

                // marking bullets by using prefab
                GameObject hitHole = Instantiate(hitHolePrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                hitHole.transform.SetParent(hit.transform);
                Destroy(hitHole, 5f);

                // knockback needs hit hole not to have a Mesh Collider
                // each hit hole needs their own mesh collider removed
                Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();
                if(rigidbody != null)
                {
                    rigidbody.AddForceAtPosition(transform.forward * 5f * knockbackPerRound, transform.position);
                }

                // hit HP reduction does so
                // each hit hole needs their own mesh collider removed
                HealthManager hitObjHP = hit.transform.GetComponent<HealthManager>();
                if(hitObjHP != null)
                {
                    hitObjHP.Damage(damagePerRound);
                }
            }
            else
            {
                // if miss
                Debug.Log(weaponName + " miss / Remaining ammo: " + currentAmmo + "/" + ammoPerMag);
            }

            anim.CrossFadeInFixedTime("Fire", fireRate);
            weaponSound.PlayOneShot(fireSE);
            CasingExitEffect();
            IncreaseScatter();

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

    private void AimDownSights()
    {
        if (Input.GetButton("Fire2") && reloading >= reloadTime)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * 8f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov / 2, Time.deltaTime * 8f);
            isAiming = true;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * 5f);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, Time.deltaTime * 8f);
            isAiming = false;
        }
    }

    void CasingExitEffect()
    {
        Quaternion randomQuaternion = new Quaternion(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f), 1);
        GameObject casing = Instantiate(bulletCasing, casingExit);
        casing.transform.localRotation = randomQuaternion;
        casing.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(Random.Range(50f, 100f), Random.Range(50f, 100f), Random.Range(-30f, 30f)));
        Destroy(casing, 1f);
    }

    void IncreaseScatter()
    {
        if(curScatter < maxScatter)
        {
            curScatter += scatterIncreasePerRound;
        }
        else
        {
            curScatter = maxScatter;
        }
    }

    void ReduceScatter()
    {
        if(curScatter > 0.0f)
        {
            curScatter -= scatterReduction * Time.deltaTime;
        }
        else
        {
            curScatter = 0.0f;
        }
    }

    void Run()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);

        if(reloading >= reloadTime)
        {
            anim.SetBool("isRunning", isRunning);
        }
    }

    void SetAmmoText()
    {
        ammoUI.text = currentAmmo + " / " + totalAmmo;
    }
}
