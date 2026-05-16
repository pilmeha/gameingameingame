using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform cameraRoot;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 100f;

    private Vector2 _lookInput;

    private float _xRotation;

    public void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }

    private void Update()
    {
        Look();
    }

    private void Look()
    {
        float mouseX = _lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = _lookInput.y * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        cameraRoot.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }
}