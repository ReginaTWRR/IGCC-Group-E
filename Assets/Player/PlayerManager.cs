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

    // Item settings 
    // アイテムに関する設定（所持数、速度上昇率、効果時間）
    [Header("Speed Item")][SerializeField] private int itemCount = 0;
    [SerializeField] private float speedUpRate = 0.15f;
    [SerializeField] private float speedUpDuration = 20f;

    // Variables used for player movement
    // プレイヤーの移動処理に使用する変数
    private CharacterController controller;
    private float verticalVelocity;
    private float cameraPitch;

    // Speed item variables 
    // 速度アップアイテム用の変数
    private float speedUpTimer = 0f;
    private bool isSpeedUp = false;

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
        Cola();
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

        // 通常の移動速度
        float currentSpeed = moveSpeed; 
        // 速度アップアイテムの効果中なら15%アップ
        if (isSpeedUp) 
        { 
            currentSpeed = moveSpeed * (1f + speedUpRate); 
        }

        //Move
        // 移動
        Vector3 velocity = move * currentSpeed + Vector3.up * verticalVelocity; controller.Move(velocity * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    //
    private void Cola()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        //  Fkey: Use item 
        // Fキー：アイテムを使用
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            // Cannot use if there is no item 
            // アイテムを持っていなければ使用できない
            if (itemCount <= 0) { Debug.Log("I don't have any items.アイテムを持っていません。"); return; }

            // Consume one item 
            // アイテムを1個消費
            itemCount--;

            // Activate speed-up effect 
            // 速度アップ効果を開始
            isSpeedUp = true;
            speedUpTimer = speedUpDuration;

            Debug.Log("I used a speed-boost item!速度アップアイテムを使用しました！");
            Debug.Log("Number of items remaining 残りアイテム数：" + itemCount);
            Debug.Log("Movement speed increases by 15% for 20 seconds.20秒間、移動速度が15%アップします。");

        }

        // Q key: Replenish item (For Functionality Testing)
        // Qキー：アイテムを補充(動作確認用)
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            // Add one item 
            // アイテムを1個補充
            itemCount++;

            Debug.Log("I restocked one item.アイテムを1個補充しました。");
            Debug.Log("Current Number of Items 現在のアイテム数：" + itemCount);

        }

        // Count down the speed-up timer 
        // 速度アップ効果の残り時間を減らす
        if (isSpeedUp)
        {
            speedUpTimer -= Time.deltaTime;
            {
                // Effect has ended
                // 効果時間が終了した場合
                if (speedUpTimer <= 0f)
                {
                    speedUpTimer = 0f; isSpeedUp = false; Debug.Log("The speed boost has expired.速度アップの効果が切れました。");
                }

            }
        }
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


