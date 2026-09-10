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
    CursorState state = CursorState.Disabled;

    public bool IsCursorEnabled => (
        state == CursorState.Enabled ||
        state == CursorState.TemporarilyEnabled
    );

    // Update is called once per frame
    void Update()
    {
        UpdateFSM();
        UpdateCursor();
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
}
