using UnityEngine;

public class CustomPlayerController : MonoBehaviour
{
    public float walkSpeed = 5f; // Wandelsnelheid
    public float runSpeed = 10f; // Rennensnelheid
    public float mouseSensitivity = 100f; // Gevoeligheid van de muis

    private float currentSpeed;
    private float xRotation = 0f;

    void Start()
    {
        // Verberg en vergrendel de cursor in het midden van het scherm
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Muisinvoer voor het roteren van de camera
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Draai de camera op de X-as (omhoog/omlaag kijken)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Zorg ervoor dat we niet te ver omhoog/omlaag kijken

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Draai de speler op de Y-as (links/rechts kijken)
        transform.Rotate(Vector3.up * mouseX);

        // Bepaal de snelheid: wandelen of rennen
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Invoer voor beweging met WASD
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Beweeg de speler in de richting waarin hij kijkt
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        transform.Translate(move * currentSpeed * Time.deltaTime, Space.World);
    }
}
