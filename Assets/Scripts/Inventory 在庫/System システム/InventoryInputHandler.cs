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
    readonly Key[] toolbarKeys =
    {
        Key.Digit1,
        Key.Digit2,
        Key.Digit3,
        Key.Digit4,
        Key.Digit5,
        Key.Digit6,
        Key.Digit7,
        Key.Digit8,
        Key.Digit9,
        Key.Digit0,
        Key.Minus,
        Key.Equals
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
}
