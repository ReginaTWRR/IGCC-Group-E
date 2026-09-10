using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("Player Camera")]
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField] float rotationSpeed = 72f;

    // Input 入力
    float mouseDeltaX = 0f;
    float mouseDeltaY = 0f;

    // Calculations 計算
    float yaw = 90f;
    float pitch = 0f;

    // Boolean flags ブールフラグ
    bool isCameraEnabled = true;

    // Update is called once per frame
    void Update()
    {
        isCameraEnabled = !CursorManager.Instance.IsCursorEnabled;
        if (isCameraEnabled == false) return;

        HandleInput();

        UpdateRotation();
        UpdatePosition();
    }

    private void HandleInput()
    {
        mouseDeltaX = Mouse.current.delta.x.ReadValue();
        mouseDeltaY = Mouse.current.delta.y.ReadValue();
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
        transform.position = player.position + offset;
    }
}
