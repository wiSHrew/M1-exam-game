using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(CharacterController), typeof(sizescaler))]
public class FirstPersonControls : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField, Min(0f)] private float walkSpeed = 5f;
    [SerializeField, Min(0f)] private float sprintSpeed = 8f;
    [SerializeField, Min(0f)] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField, Min(0f)] private float mouseSensitivity = 0.1f;
    [SerializeField, Range(0f, 89f)] private float maxLookAngle = 85f;
    [SerializeField] private Key sizeScalerHoldKey = Key.Y;
    [SerializeField] private Key speedScalerHoldKey = Key.K;
    [SerializeField, Min(0f)] private float speedScrollSensitivity = 0.1f;
    [SerializeField, Min(0.01f)] private float minimumSpeedMultiplier = 0.25f;
    [SerializeField, Min(0.01f)] private float maximumSpeedMultiplier = 3f;
    [SerializeField] private float fallRecoveryThreshold = -10f;
    [SerializeField] private float fallRecoveryHeight = 10f;
    [SerializeField] private TMP_Text speedDisplay;
    [SerializeField] private TMP_Text sizeDisplay;

    private CharacterController characterController;
    private sizescaler sizeScaler;
    private float verticalVelocity;
    private float cameraPitch;
    private float speedMultiplier = 1f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        sizeScaler = GetComponent<sizescaler>();

        if (sizeScaler == null)
        {
            Debug.LogError("A sizescaler component is required on the player GameObject.", this);
        }

        if (speedDisplay == null)
        {
            Debug.LogError("Assign a TextMeshPro component to the speed display field.", this);
        }

        if (sizeDisplay == null)
        {
            Debug.LogError("Assign a TextMeshPro component to the size display field.", this);
        }

        if (playerCamera == null)
        {
            Camera childCamera = GetComponentInChildren<Camera>();
            if (childCamera != null)
            {
                playerCamera = childCamera.transform;
            }
        }

        if (playerCamera == null)
        {
            Debug.LogError("Assign a player camera or add a Camera as a child.", this);
            enabled = false;
        }
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (transform.position.y < fallRecoveryThreshold)
        {
            characterController.enabled = false;
            Vector3 position = transform.position;
            position.y = fallRecoveryHeight;
            transform.position = position;
            characterController.enabled = true;
            verticalVelocity = 0f;
        }

        Keyboard keyboard = Keyboard.current;
        Vector2 moveInput = Vector2.zero;
        bool sprinting = false;
        bool jumpPressed = false;

        if (keyboard != null)
        {
            moveInput.x = (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed ? 1f : 0f)
                - (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed ? 1f : 0f);
            moveInput.y = (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed ? 1f : 0f)
                - (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed ? 1f : 0f);
            sprinting = keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed;
            jumpPressed = keyboard.spaceKey.wasPressedThisFrame;
        }

        if (sizeScaler != null)
        {
            sizeScaler.SetScalingEnabled(
                keyboard != null && keyboard[sizeScalerHoldKey].isPressed);
        }

        if (keyboard != null && keyboard[speedScalerHoldKey].isPressed)
        {
            float scroll = Input.mouseScrollDelta.y;
            if (!Mathf.Approximately(scroll, 0f))
            {
                speedMultiplier = Mathf.Clamp(
                    speedMultiplier * (1f + scroll * speedScrollSensitivity),
                    minimumSpeedMultiplier,
                    maximumSpeedMultiplier);
            }
        }

        Vector2 lookInput = Mouse.current != null
            ? Mouse.current.delta.ReadValue() * mouseSensitivity
            : Vector2.zero;

        transform.Rotate(Vector3.up, lookInput.x, Space.World);
        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -maxLookAngle, maxLookAngle);
        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

        if (characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        if (characterController.isGrounded && jumpPressed)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 movement = transform.right * moveInput.x + transform.forward * moveInput.y;
        movement = Vector3.ClampMagnitude(movement, 1f);
        float speed = (sprinting ? sprintSpeed : walkSpeed) * speedMultiplier;

        characterController.Move(
            (movement * speed + Vector3.up * verticalVelocity) * Time.deltaTime);

        if (speedDisplay != null)
        {
            speedDisplay.SetText("Speed: {0:0.0}", speed);
        }

        if (sizeDisplay != null && sizeScaler != null)
        {
            sizeDisplay.SetText("Size: {0:0.0}x", sizeScaler.CurrentScaleMultiplier);
        }
    }
}
