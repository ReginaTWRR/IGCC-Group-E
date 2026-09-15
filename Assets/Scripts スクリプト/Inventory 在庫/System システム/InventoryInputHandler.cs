using UnityEngine;
using UnityEngine.InputSystem;

public enum ScrollMovement
{
    None,
    Upwards,
    Downwards
}

public class InventoryInputHandler : PersistentSingleton<InventoryInputHandler>
{
    // Keep a list of all the keybinds that will be used to select a box in the toolbar
    // ツールバー内のボックスを選択するために使用するすべてのキーバインドのリストを保持してください。
    readonly UnityEngine.InputSystem.Key[] toolbarKeys =
    {
        UnityEngine.InputSystem.Key.Digit1,
        UnityEngine.InputSystem.Key.Digit2,
        UnityEngine.InputSystem.Key.Digit3,
        UnityEngine.InputSystem.Key.Digit4,
        UnityEngine.InputSystem.Key.Digit5,
        UnityEngine.InputSystem.Key.Digit6,
        UnityEngine.InputSystem.Key.Digit7,
        UnityEngine.InputSystem.Key.Digit8,
        UnityEngine.InputSystem.Key.Digit9,
        UnityEngine.InputSystem.Key.Digit0,
        UnityEngine.InputSystem.Key.Minus,
        UnityEngine.InputSystem.Key.Equals
    };

    // Store the selected key index
    // 選択されたキーのインデックスを保存します
    int selectedKeyIndex = 0;
    public int SelectedKeyIndex => selectedKeyIndex;

    public bool CheckKeySelection()
    {
        for (int i = 0; i < toolbarKeys.Length; ++i)
        {
            if (Keyboard.current[toolbarKeys[i]].wasPressedThisFrame)
            {
                selectedKeyIndex = i;
                return true;
            }
        }

        return false;
    }

    public ScrollMovement CheckMouseScroll()
    {
        Vector2 mouseScroll = Mouse.current.scroll.ReadValue();
        
        if (mouseScroll.y > 0f)
        {
            return ScrollMovement.Upwards;
        }
        else if (mouseScroll.y < 0f)
        {
            return ScrollMovement.Downwards;
        }

        return ScrollMovement.None;
    }

    public bool CheckCollectItemPressed()
    {
        return InputSystem.actions["Collect Item"].WasPressedThisFrame();
    }

    public bool CheckUseItemPressed()
    {
        return InputSystem.actions["Use Item"].WasPressedThisFrame();
    }

    public bool CheckToggleInventoryPressed()
    {
        return InputSystem.actions["Toggle Inventory"].WasPressedThisFrame();
    }
}
