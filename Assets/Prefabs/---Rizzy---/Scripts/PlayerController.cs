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
    public bool isAiming = false;
    public Animator animator;

    public GameObject Crosshair;

    public float normalFOV = 60f; // Normal Field of View
    public float aimingFOV = 30f; // Aiming Field of View
    public float fovTransitionSpeed = 10f; // Speed of FOV transition

    private Camera playerCamera;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        animator = GetComponent<Animator>(); // Ensure your Animator is on the same GameObject
        playerCamera = cameraTransform.GetComponent<Camera>();
        playerCamera.fieldOfView = normalFOV;
    }

    void Update()
    {
        Move();
        Jump();
        LookAround();
        Aim();
        AdjustFOV();
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

    void Aim()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;
            Crosshair.SetActive(false);
            animator.SetBool("isAiming", true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            Crosshair.SetActive(true);
            animator.SetBool("isAiming", false);
        }
    }

    void AdjustFOV()
    {
        if (isAiming)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, aimingFOV, fovTransitionSpeed * Time.deltaTime);
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, fovTransitionSpeed * Time.deltaTime);
        }
    }
}
