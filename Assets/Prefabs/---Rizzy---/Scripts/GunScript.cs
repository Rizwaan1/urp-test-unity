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
    public AudioClip overheatSound; // Add an overheat sound
    public int maxAmmo = 30;
    public int currentAmmo;
    public float fireRate = 0.1f;
    public float reloadTime = 1.5f;
    public bool isAutomatic = true;

    private Vector3 initialPosition;
    private bool isReloading = false;
    private float nextTimeToFire = 0f;
    public Text Ammo;

    public GameObject bulletPrefab, bulletVisual, muzzleFlash; // De prefab die je wilt instantiate
    public Transform firePoint; // Het punt van waaruit de prefab wordt geinstantieerd

    public bool rayCast;
    public bool instantiateFire;

    private CameraRecoil Recoil_script;

    // Hipfire Recoil
    [SerializeField] public float recoilX;
    [SerializeField] public float recoilY;
    [SerializeField] public float recoilZ;

    // ADS Recoil
    [SerializeField] public float aimrecoilX;
    [SerializeField] public float aimrecoilY;
    [SerializeField] public float aimrecoilZ;

    // Settings
    [SerializeField] public float snappiness;
    [SerializeField] public float returnSpeed;

    // Heat System
    public float heat = 0f;
    public float maxHeat = 100f;
    public float heatIncreasePerShot = 5f;
    public float heatDecreaseRate = 2f;
    public float minPitch = 1f;
    public float maxPitch = 3f;
    public bool isOverheated = false;
    public float cooldownDelay = 2f; // Time before heat starts to decrease
    private float lastShotTime;
    public float overheatCooldownTime = 5f; // Time to wait before the gun can shoot again after overheating

    void Start()
    {
        initialPosition = transform.localPosition;
        currentAmmo = maxAmmo;
        Ammo.text = Mathf.RoundToInt(currentAmmo).ToString(); // Toon health als geheel getal
        Recoil_script = GameObject.Find("Camera Rotation/Camera Recoil").GetComponent<CameraRecoil>();
    }

    void Update()
    {
        if (isReloading)
            return;

        HandleSway();
        HandleHeat();

        if (isOverheated)
            return;

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
                if (rayCast)
                {
                    ShootRaycast();
                }
                if (instantiateFire)
                {
                    ShootInsantiate();
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                if (rayCast)
                {
                    ShootRaycast();
                }
                if (instantiateFire)
                {
                    ShootInsantiate();
                }
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

    void HandleHeat()
    {
        if (Time.time - lastShotTime >= cooldownDelay && heat > 0 && !isOverheated)
        {
            heat -= heatDecreaseRate * Time.deltaTime;
            heat = Mathf.Clamp(heat, 0, maxHeat);
        }

        // Adjust the pitch based on the current heat level
        source.pitch = Mathf.Lerp(minPitch, maxPitch, heat / maxHeat);

        if (heat >= maxHeat)
        {
            StartCoroutine(Overheat());
        }
    }

    void IncreaseHeat()
    {
        heat += heatIncreasePerShot;
        heat = Mathf.Clamp(heat, 0, maxHeat);
        lastShotTime = Time.time;
    }

    IEnumerator Overheat()
    {
        isOverheated = true;
        source.PlayOneShot(overheatSound);
        Debug.Log("Overheated...");

        yield return new WaitForSeconds(overheatCooldownTime);

        isOverheated = false;
        heat = 0; // Reset heat after cooldown
    }

    void ShootRaycast()
    {
        IncreaseHeat();
        source.PlayOneShot(gunShot);
        ShootBulletVisual();
        currentAmmo--;
        Ammo.text = Mathf.RoundToInt(currentAmmo).ToString(); // Toon health als geheel getal
        Recoil_script.RecoilFire();

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

    void ShootBulletVisual()
    {
        MuzzleFlash();
        Instantiate(bulletVisual, firePoint.position, firePoint.rotation);
    }

    void MuzzleFlash()
    {
        Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
    }

    void ShootInsantiate()
    {
        IncreaseHeat();
        source.PlayOneShot(gunShot);
        MuzzleFlash();
        currentAmmo--;
        Ammo.text = Mathf.RoundToInt(currentAmmo).ToString(); // Toon health als geheel getal
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Recoil_script.RecoilFire();
    }
}
