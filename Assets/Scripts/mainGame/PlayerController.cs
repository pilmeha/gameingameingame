using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float gravity = -9.81f;

    private CharacterController _controller;

    private Vector2 _moveInput;
    private float _currentSpeed;

    private Vector3 _velocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        _currentSpeed = walkSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnSprint(InputValue value)
    {
        _currentSpeed = value.isPressed
            ? sprintSpeed
            : walkSpeed;
    }

    private void Update()
    {
        Move();
        ApplyGravity();
    }

    private void Move()
    {
        Vector3 move =
            transform.forward * _moveInput.y +
            transform.right * _moveInput.x;

        _controller.Move(move * _currentSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += gravity * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);
    }
}