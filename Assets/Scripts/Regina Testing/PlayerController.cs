using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Rigidbody rb;
    [SerializeField] float moveSpeed = 10f;
    Vector3 moveDirection;

    // Input 入力
    bool isFrontDown = false;
    bool isBackDown = false;
    bool isRightDown = false;
    bool isLeftDown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdateDirection();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    private void HandleInput()
    {
        isFrontDown = Keyboard.current.wKey.isPressed;
        isBackDown = Keyboard.current.sKey.isPressed;
        isRightDown = Keyboard.current.dKey.isPressed;
        isLeftDown = Keyboard.current.aKey.isPressed;
    }

    private void UpdateDirection()
    {
        moveDirection = Vector3.zero;

        if (isFrontDown) moveDirection += Camera.main.transform.forward;
        if (isBackDown) moveDirection -= Camera.main.transform.forward;
        if (isRightDown) moveDirection += Camera.main.transform.right;
        if (isLeftDown) moveDirection -= Camera.main.transform.right;

        moveDirection.y = 0f;
        moveDirection.Normalize();
    }
}
