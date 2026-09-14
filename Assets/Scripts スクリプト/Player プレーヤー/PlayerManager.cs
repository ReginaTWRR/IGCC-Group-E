using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : PersistentSingleton<Player>
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

    // Health settings
    // 健康設定
    [Header("Health")]
    [SerializeField] float startingHealth = 100f;
    [SerializeField] float maxHealth = 100f;
    private float currentHealth;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    // Item settings 
    // アイテムに関する設定（所持数、速度上昇率、効果時間）
    [Header("Speed Item")][SerializeField] private int itemCount = 0;
    private float speedUpRate;
    private float speedUpDuration;

    [SerializeField] private AudioSource footstepAudio;
    [SerializeField] private float footstepInterval = 0.4f;

    private float footstepTimer = 0f;

    // Variables used for player movement
    // プレイヤーの移動処理に使用する変数
    private CharacterController controller;
    private float verticalVelocity;
    private float cameraPitch;

    // Variables used for camera movement
    // カメラの動きに使用される変数
    public bool ShouldCameraMove => (
        GameManager.Instance.IsGamePaused == false &&
        CursorManager.Instance.IsCursorEnabled == false
    );

    // Speed item variables 
    // 速度アップアイテム用の変数
    private float speedUpTimer = 0f;
    private bool isSpeedUp = false;

    protected override void Awake()
    {
        base.Awake();

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

        // Initialise the health
        // ヘルスを初期化する
        if (startingHealth > maxHealth)
        {
            Debug.LogError("PlayerManager: Attempted to start the game with too much health.");
        }

        currentHealth = startingHealth;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HealthUI.Instance.UpdateHealthUI(currentHealth);
    }

    private void Update()
    {
        Move();
        Look();
        Cola();
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        HealthUI.Instance.UpdateHealthUI(currentHealth);
    }

    public void SpeedUp(float speedUpRate, float speedUpDuration)
    {
        this.speedUpRate = speedUpRate;
        this.speedUpDuration = speedUpDuration;

        speedUpTimer = speedUpDuration;
        isSpeedUp = true;
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

        // WASDを押している間だけ効果音を再生
        if (input.sqrMagnitude > 0.01f)
        {
            if (footstepAudio != null && !footstepAudio.isPlaying)
            {
                footstepAudio.Play();
            }
        }
        else
        {
            // WASDを離したら効果音を停止
            if (footstepAudio != null && footstepAudio.isPlaying)
            {
                footstepAudio.Stop();
            }
        }


        //Default Movement Speed Setting 通常の移動速度設定
        float currentSpeed = moveSpeed;
        //If a speed-boosting item is active, speed is increased by 15%　速度アップアイテムの効果中なら15%アップ
        if (isSpeedUp)
        {
            currentSpeed = moveSpeed * (1f + speedUpRate);
        }

        // Detection While in Ghost Mode (CharacterController Disabled)
        if (controller != null && !controller.enabled)
        {
            float deltaTime = Time.deltaTime;

            //Calculating the CharacterController's Foot Position
            // CharacterControllerの足元位置を計算
            float controllerBottom = controller.center.y - controller.height * 0.5f;

            Vector3 feetPosition = transform.position + Vector3.up * controllerBottom;

            // Fire a ray from a point slightly above the feet
            // 足元より少し上からRayを飛ばす
            float rayStartOffset = 0.1f;
            float rayLength = 0.3f;

            Vector3 rayOrigin = feetPosition + Vector3.up * rayStartOffset;
            bool isGroundedGhost = Physics.Raycast(rayOrigin,Vector3.down,out RaycastHit hit,rayLength);

            if (isGroundedGhost && verticalVelocity <= 0.1f)
            {
                // Calculate the distance between the current foot and the floor
                // 現在の足元と床との距離を計算
                float groundOffset = hit.point.y - feetPosition.y;

                // Align your feet with the floor
                // 足元を床に合わせる
                transform.position += Vector3.up * groundOffset;

                // Maintain ground connection
                // 接地状態を維持
                verticalVelocity = -1.0f;

                //Jump
                // ジャンプ
                if (Keyboard.current != null &&
                    InputSystem.actions["Jump"].WasPressedThisFrame())
                {
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
            }
            else
            {
                // In the air
                // 空中
                verticalVelocity += gravity * deltaTime;
            }

            //Move
            // 移動
            Vector3 ghostVelocity = move * currentSpeed + Vector3.up * verticalVelocity;
            transform.Translate(ghostVelocity * deltaTime, Space.World);

            return;
        }

        //Physics and Movement Processing in Human State (with CharacterController enabled)　人間状態（CharacterControllerが有効）の物理・移動処理
        bool isGrounded = controller.isGrounded;

        if (isGrounded)
        {
            verticalVelocity = -2f;

            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // Gravity
        // 重力
        verticalVelocity += gravity * Time.deltaTime;

        //Movement with Collision Detection Using CharacterController CharacterControllerによる衝突判定ありの移動
        Vector3 velocity = move * currentSpeed + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);


    }


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
        // Check if the camera should move
        // カメラを移動させるべきかどうかを確認する
        if (ShouldCameraMove == false) return;

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
