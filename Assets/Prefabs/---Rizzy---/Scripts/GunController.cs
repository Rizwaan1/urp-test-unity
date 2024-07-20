using UnityEngine;

public class GunController : MonoBehaviour
{
    public float fireRate = 10f; // Rounds per second
    public float recoilAmount = 2f; // Amount of recoil
    public Transform cameraTransform; // Reference to the camera
    public Transform gunTransform; // Reference to the gun
    public GameObject bulletPrefab; // Prefab for the bullet
    public Transform bulletSpawnPoint; // Point where the bullets are spawned
    public float bulletSpeed = 20f; // Speed of the bullet
    public AudioClip shootSound; // The sound to play when shooting

    private float nextTimeToFire = 0f;
    private Vector3 initialGunPosition;
    private AudioSource audioSource;

    void Start()
    {
        initialGunPosition = gunTransform.localPosition;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        ApplyRecoil();
    }

    void Shoot()
    {
        // Instantiate and shoot the bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bulletSpawnPoint.forward * bulletSpeed;

        // Play shoot sound
        audioSource.PlayOneShot(shootSound);

        
    }

    void ApplyRecoil()
    {
       
        
    }
}
