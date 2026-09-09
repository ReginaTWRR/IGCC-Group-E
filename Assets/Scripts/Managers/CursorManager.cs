using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : PersistentSingleton<CursorManager>
{
    bool isCursorEnabled = false;
    public bool IsCursorEnabled => isCursorEnabled;

    protected override void Awake()
    {
        base.Awake();
        isCursorEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdateCursor();
    }

    private void HandleInput()
    {
        isCursorEnabled = Keyboard.current.leftAltKey.isPressed;
    }

    private void UpdateCursor()
    {
        if (isCursorEnabled)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
