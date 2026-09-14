using UnityEngine;
using UnityEngine.InputSystem;

public enum CursorState
{
    Enabled,
    Disabled,
    TemporarilyEnabled
}

public class CursorManager : PersistentSingleton<CursorManager>
{
    // Variables controlling the state of the cursor
    // カーソルの状態を制御する変数
    CursorState state;

    public bool IsCursorEnabled => (
        state == CursorState.Enabled ||
        state == CursorState.TemporarilyEnabled
    );

    // Variables for hovering over objects
    // オブジェクトにマウスカーソルを合わせるための変数
    [Header("Hovering")]
    [SerializeField] LayerMask raycastTargets;

    Camera mainCam;
    GameObject hoveredObject;

    public GameObject HoveredObject => hoveredObject;

    protected override void Awake()
    {
        base.Awake();
        state = CursorState.Disabled;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Cache the main camera for better performance
        // パフォーマンス向上のため、メインカメラをキャッシュする
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFSM();
        UpdateCursor();

        if (IsCursorEnabled == false) return;

        DetectHover();
    }

    private void UpdateFSM()
    {
        switch (state)
        {
            case CursorState.Enabled:
                if (InventoryUI.Instance.IsInventoryOpen == false)
                {
                    state = CursorState.Disabled;
                }

                break;
            case CursorState.Disabled:
                if (InventoryUI.Instance.IsInventoryOpen)
                {
                    state = CursorState.Enabled;
                }
                else if (Keyboard.current.leftAltKey.isPressed)
                {
                    state = CursorState.TemporarilyEnabled;
                }

                break;
            case CursorState.TemporarilyEnabled:
                if (Keyboard.current.leftAltKey.isPressed == false)
                {
                    state = CursorState.Enabled;
                }

                break;
        }
    }

    private void UpdateCursor()
    {
        // Update the cursor
        // カーソルを更新する
        if (IsCursorEnabled)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void DetectHover()
    {
        // Create a ray from the camera passing through the mouse position
        // カメラからマウスの位置を通過する光線を作成する
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());

        // Do the raycast
        // レイキャストを実行する
        if (Physics.Raycast(ray, out RaycastHit hit, raycastTargets))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != hoveredObject)
            {
                hoveredObject = hitObject;
            }
        }
        else
        {
            hoveredObject = null;
        }
    }
}
