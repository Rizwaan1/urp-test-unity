using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Gun : MonoBehaviour
{
    public int maxAmmo = 30;
    public int currentAmmo;
    public float reloadTime = 1f;
    public float fireRate = 0.1f;
    public float recoilForce = 5f;
    public float range = 100f;
    public float damage = 10f;

    public Transform bulletSpawn;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private bool isReloading = false;
    private float nextTimeToFire = 0f;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void OnEnable()
    {
        isReloading = false;
    }

    private void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void Shoot()
    {
        currentAmmo--;

        // Muzzle flash effect
        muzzleFlash.Play();

        // Apply recoil
        ApplyRecoil();

        // Raycast for shooting
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            // Deal damage if target has health component
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            // Instantiate impact effect
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }

        Debug.Log("Shot fired. Ammo left: " + currentAmmo);
    }

    void ApplyRecoil()
    {
        // Apply a recoil force to the camera
        fpsCam.transform.localPosition -= new Vector3(0, 0, recoilForce * Time.deltaTime);
    }
}
