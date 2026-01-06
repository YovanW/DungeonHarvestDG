using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 2.5f;
    public float gravity = -9.81f;

    [Header("Dodge/Dash Settings")]
    public float dashForce = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Camera Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;

    [Header("Stamina Settings")]
    public float jumpStaminaCost = 15f;
    public float dashStaminaCost = 1f;
    public float minStaminaToSprint = 10f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isDashing;
    private float dashTime;
    private float dashCooldownTimer;
    private Vector3 dashDirection;
    private float xRotation = 0f;
    private HealthStaminaManager healthStaminaManager;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>().transform;
        }

        healthStaminaManager = GetComponent<HealthStaminaManager>();
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleJump();
        HandleDodge();
        ApplyGravity();
    }

    public void ResetLook()
    {
        xRotation = 0f;

        // reset rotasi camera (pitch)
        if (playerCamera != null)
            playerCamera.localRotation = Quaternion.identity;

        // reset rotasi player (yaw)
        transform.rotation = Quaternion.identity;
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        if (isDashing) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        bool canSprint = healthStaminaManager == null || healthStaminaManager.currentStamina > minStaminaToSprint;
        bool trySprint = Input.GetKey(KeyCode.LeftShift) && canSprint;

        float currentSpeed = trySprint ? sprintSpeed : walkSpeed;

        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    private float lastJumpTime;
    public float jumpCooldown = 0.2f;

    private int jumpCount = 0;
    private int maxJumps = 1; // For double jump

    void HandleJump()
    {
        // Reset jump count when grounded
        if (controller.isGrounded)
        {
            jumpCount = 0;
        }

        if (Time.time < lastJumpTime + jumpCooldown) return;

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            if (healthStaminaManager != null)
            {
                if (!healthStaminaManager.UseStamina(jumpStaminaCost))
                    return;
            }

            Debug.Log("JUMP " + (jumpCount + 1));

            velocity.y = 0;
            velocity.y = Mathf.Sqrt(jumpForce * -1f * gravity);

            lastJumpTime = Time.time;
            jumpCount++;
        }
    }

    void HandleDodge()
    {
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.C) && dashCooldownTimer <= 0)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f)
            {
                if (healthStaminaManager != null)
                {
                    if (!healthStaminaManager.UseStamina(dashStaminaCost))
                        return;
                }

                StartDodge(new Vector3(x, 0, z).normalized);
            }
        }

        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            controller.Move(dashDirection * dashForce * Time.deltaTime);

            if (dashTime <= 0)
            {
                isDashing = false;
                dashCooldownTimer = dashCooldown;
            }
        }
    }

    void StartDodge(Vector3 direction)
    {
        isDashing = true;
        dashTime = dashDuration;
        dashDirection = transform.right * direction.x + transform.forward * direction.z;
    }

    void ApplyGravity()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}