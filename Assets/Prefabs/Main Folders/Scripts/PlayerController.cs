using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 100f;
    public float airControlFactor = 0.5f;  // Factor to control movement in the air

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    public Transform cameraTransform;
    public bool Stasis;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        Jump();
        LookAround();
    }

    void Move()
    {
        if (!Stasis)
        {
            isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // Small downward force to keep the player grounded
            }

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 move = transform.right * moveX + transform.forward * moveZ;

            // Normalize the movement vector to prevent faster diagonal movement
            if (move.magnitude > 1)
            {
                move.Normalize();
            }

            if (isGrounded)
            {
                controller.Move(move * moveSpeed * Time.deltaTime);
            }
            else
            {
                // Apply reduced horizontal control in the air
                Vector3 airMove = new Vector3(move.x * airControlFactor, 0, move.z * airControlFactor);
                controller.Move(airMove * moveSpeed * Time.deltaTime);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        
    }

    void Jump()
    {
        if (!Stasis)
        {
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        
    }

    void LookAround()
    {
        if (!Stasis)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
        
    }
}
