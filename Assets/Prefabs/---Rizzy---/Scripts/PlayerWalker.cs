using UnityEngine;

public class PlayerWalker : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 100f;
    public float minXRotation = -90f;  // Minimale hoek omhoog kijken
    public float maxXRotation = 90f;   // Maximale hoek omlaag kijken

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
    }

    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotatie om de X-as (omhoog/omlaag kijken) met instelbare clamp
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minXRotation, maxXRotation);

        // Pas de rotatie van de camera aan op de X-as
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotatie om de Y-as (links/rechts draaien) toegepast op de spelerf
        transform.Rotate(Vector3.up * mouseX);
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }
}
