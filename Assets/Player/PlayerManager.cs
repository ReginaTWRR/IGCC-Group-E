using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    // Movement settings (movement speed, jump height, gravity)
    // 移動に関する設定（移動速度、ジャンプの高さ、重力の重さ）
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    // Mouse control settings (camera used for the viewpoint, ease of movement for the viewpoint, maximum angle the camera can move up and down)
    // マウス操作に関する設定（視点になるカメラ、視点の動きやすさ、カメラを上下に動かせる最大角度）
    [Header("Mouse Look")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float maxLookAngle = 90f;

    // Variables used for player movement
    // プレイヤーの移動処理に使用する変数
    private CharacterController controller;
    private float verticalVelocity;
    private float cameraPitch;

    private void Awake()
    {
        // Get the CharacterController component
        // CharacterControllerコンポーネントを取得する
        controller = GetComponent<CharacterController>();

        if (playerCamera == null)
        {
            // Search for a Camera component among the player's own child objects
            // プレイヤー自身の子オブジェクトの中からCameraコンポーネントを探す
            Camera cam = GetComponentInChildren<Camera>();

            // If a camera is found
            // カメラが見つかった場合
            if (cam != null)
            {
                // Set this to playerCamera
                // playerCameraに設定する
                playerCamera = cam.transform;
            }
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        // Movement using WASD
        // WASDによる移動
        Vector2 input = Vector2.zero;
        if (Keyboard.current != null)
        {
            // W
            if (Keyboard.current.wKey.isPressed)
                input.y += 1f;

            // S
            if (Keyboard.current.sKey.isPressed)
                input.y -= 1f;

            // A
            if (Keyboard.current.aKey.isPressed)
                input.x -= 1f;

            // D
            if (Keyboard.current.dKey.isPressed)
                input.x += 1f;
        }

        // Prevent diagonal movement from becoming too fast
        // 斜め移動が速くなりすぎないようにする
        input = Vector2.ClampMagnitude(input, 1f);
        Vector3 move = transform.right * input.x + transform.forward * input.y;

        // Ground Collision Detection
        // 地面判定
        bool isGrounded = controller.isGrounded;

        // Jump
        // ジャンプ
        if (isGrounded == true)
        {
            // Reset the falling speed when on the ground
            // 地面にいるときは落下速度をリセット
            verticalVelocity = -2f;

            // The moment the Space key is pressed
            // Spaceキーが押された瞬間
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                // Set jump speed
                // ジャンプ速度を設定
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // Gravity
        // 重力
        verticalVelocity += gravity * Time.deltaTime;

        //Move
        // 移動
        Vector3 velocity = move * moveSpeed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);

        //アイテムを使ったら足が速くなる
    }

    private void Look()
    {
        // Check if the mouse is working
        //マウスが使える状態か確認する
        if (Mouse.current == null)
        {
            // If that doesn't work, exit
            //できなかったら終了
            return;
        }

        // Get how much the frame has moved since the previous frame
        //前のフレームからどれだけ動いたかを取得する
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        // Get the distance the mouse has moved (X-axis, Y-axis)
        //マウスが移動した距離の取得（X軸、Y軸）
        float mouseX = mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;

        // Rotate the player character left and right
        // プレイヤー本体を左右に回転
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera up and down
        // カメラを上下に回転
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);
        playerCamera.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

}
