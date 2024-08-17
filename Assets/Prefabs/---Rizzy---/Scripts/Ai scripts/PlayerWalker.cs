using UnityEngine;

public class PlayerWalker : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 100f;
    public float minXRotation = -90f;  // Minimale hoek omhoog kijken
    public float maxXRotation = 90f;   // Maximale hoek omlaag kijken
    public Animator animator; // Reference to the Animator component

    public GameObject bulletPrefab; // Reference to the bullet prefab
    public Transform firePoint; // The point from where the bullet is fired
    public float bulletForce = 20f; // The speed at which the bullet moves

    private float xRotation = 0f;

    void Start()
    {
        // Verberg de muisaanwijzer en lock de cursor in het midden van het scherm
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Rotate();
        Move();
        HandleMouseInput();
    }

    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotatie om de X-as (omhoog/omlaag kijken) met instelbare clamp
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minXRotation, maxXRotation);

        // Pas de rotatie van de camera aan op de X-as en de Y-as
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotatie om de Y-as (links/rechts draaien) toegepast op de speler
        transform.Rotate(Vector3.up * mouseX);

        // Apply the vertical rotation to the player's body along with the camera
        transform.localRotation = Quaternion.Euler(xRotation, transform.localEulerAngles.y, 0f);
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }

    void HandleMouseInput()
    {
        // Right mouse button pressed - switch to aim idle animation
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("IsAiming", true);
        }
        // Right mouse button released - stop aiming
        else if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("IsAiming", false);
        }

        // Left mouse button pressed - play attack animation and fire bullet
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
            FireBullet();
        }
    }

    void FireBullet()
    {
        // Instantiate the bullet at the fire point position and rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Get the Rigidbody component of the bullet and apply force to it
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }
}
