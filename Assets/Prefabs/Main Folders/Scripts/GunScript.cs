using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float swayAmount = 0.05f;
    public float swaySpeed = 2.0f;
    public Camera fpsCam;
    public AudioSource source;
    public AudioClip gunShot;
    public AudioClip reloadSound;
    public int maxAmmo = 30;
    public int currentAmmo;
    public float fireRate = 0.1f;
    public float reloadTime = 1.5f;
    public bool isAutomatic = true;

    private Vector3 initialPosition;
    private bool isReloading = false;
    private float nextTimeToFire = 0f;
    public Text Ammo;

    void Start()
    {
        initialPosition = transform.localPosition;
        currentAmmo = maxAmmo;
        Ammo.text = Mathf.RoundToInt(currentAmmo).ToString(); // Toon health als geheel getal
    }

    void Update()
    {
        
        if (isReloading)
            return;

        HandleSway();

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (isAutomatic)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    void HandleSway()
    {
        float moveX = Input.GetAxis("Mouse X") * swayAmount;
        float moveY = Input.GetAxis("Mouse Y") * swayAmount;

        Vector3 finalPosition = new Vector3(moveX, moveY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * swaySpeed);
    }

    void Shoot()
    {
        source.PlayOneShot(gunShot);
        currentAmmo--;
        Ammo.text = Mathf.RoundToInt(currentAmmo).ToString(); // Toon health als geheel getal


        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        source.PlayOneShot(reloadSound);
        
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
