using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("Player Camera")]
    [SerializeField] Transform player;
    [SerializeField] float rotationSpeed = 72f;

    // Input 入力
    float mouseDeltaX = 0f;
    float mouseDeltaY = 0f;

    Vector2 scrollInput;

    // Calculations 計算
    float yaw = 0f;
    float pitch = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        UpdateRotation();
        UpdatePosition();
    }

    private void HandleInput()
    {
        mouseDeltaX = Mouse.current.delta.x.ReadValue();
        mouseDeltaY = Mouse.current.delta.y.ReadValue();

        scrollInput = Mouse.current.scroll.ReadValue();
    }

    private void UpdateRotation()
    {
        float finalRotationSpeed = rotationSpeed / 2f;

        yaw += mouseDeltaX * finalRotationSpeed * Time.deltaTime;
        pitch += mouseDeltaY * finalRotationSpeed * Time.deltaTime;

        // Prevent gimbal lock
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.rotation = Quaternion.Euler(-pitch, yaw, 0f);
    }

    private void UpdatePosition()
    {
        transform.position = player.position;
        transform.LookAt(player.position);
    }
}
