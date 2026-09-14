using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _cameraTransform;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 4f;

    [Header("Mouse Look Settings")]
    [SerializeField] private float _mouseSensitivity = 0.15f;
    [SerializeField] private float _maxLookAngle = 85f;

    [Header("Gravity")]
    [SerializeField] private float _gravity = -20f;

    private CharacterController _characterController;

    private float _verticalVelocity;
    private float _cameraPitch;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    private void HandleMovement()
    {
        if (Keyboard.current == null) return;

        Vector2 movementInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) movementInput.y += 1f;
        if (Keyboard.current.sKey.isPressed) movementInput.y -= 1f;
        if (Keyboard.current.dKey.isPressed) movementInput.x += 1f;
        if (Keyboard.current.aKey.isPressed) movementInput.x -= 1f;

        movementInput = Vector2.ClampMagnitude(movementInput, 1f);

        Vector3 movementDirection = transform.right * movementInput.x + transform.forward * movementInput.y;

        if (_characterController.isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f;
        }

        _verticalVelocity += _gravity * Time.deltaTime;

        movementDirection *= _moveSpeed;
        movementDirection.y = _verticalVelocity;

        _characterController.Move(movementDirection * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        if (Mouse.current == null) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * _mouseSensitivity;
        float mouseY = mouseDelta.y * _mouseSensitivity;

        // Rotate player left / right
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera up / down
        _cameraPitch -= mouseY;

        _cameraPitch = Mathf.Clamp(_cameraPitch, -_maxLookAngle, _maxLookAngle);

        _cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
