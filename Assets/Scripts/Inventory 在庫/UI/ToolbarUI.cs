using UnityEngine;

public class ToolbarUI : PersistentSingleton<ToolbarUI>
{
    [Header("Toolbar")]
    // Store a reference to the inventory row
    // 在庫行への参照を保存します
    [SerializeField] InventoryRowUI row;
    // Store a reference to the selection outline
    // 選択範囲のアウトラインへの参照を保存します
    [SerializeField] SelectionOutlineUI slnOutline;

    // Store the selected slot index
    // 選択されたスロットのインデックスを保存します
    int selectedSlotIndex = 0;

    // Boolean flag to indicate whether the mouse is lifting an item
    // マウスがアイテムを運んでいるかどうかを示すブール値フラグ
    bool isLiftingItem = false;

    // Update is called once per frame
    void Update()
    {
        UpdateSelection();
    }

    public void OnInventorySlotClicked(InventorySlotUI slotClicked)
    {
        if (!isLiftingItem)
        {
            if (slotClicked.IsOccupied)
            {
                // Lift the item
                // アイテムを持ち上げる
                slotClicked.ClearUI();
            }
        }
        else
        {

        }
    }

    private void UpdateSelection()
    {
        // Receive the input values from InventoryInputHandler
        // InventoryInputHandlerから入力値を受け取ります
        if (InventoryInputHandler.Instance.CheckKeySelection())
        {
            selectedSlotIndex = InventoryInputHandler.Instance.SelectedKeyIndex;
        }

        ScrollMovement scroll = InventoryInputHandler.Instance.CheckMouseScroll();

        // Interpret the values
        // 値を解釈する
        if (scroll == ScrollMovement.Upwards)
        {
            if (selectedSlotIndex < row.slots.Count - 1) ++selectedSlotIndex;
        }
        else if (scroll == ScrollMovement.Downwards)
        {
            if (selectedSlotIndex > 0) --selectedSlotIndex;
        }

        // Move the selection outline accordingly
        // 選択範囲の輪郭をそれに応じて移動します
        slnOutline.MoveSelectionOutline(row.slots[selectedSlotIndex]);
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the inventory row
        // 在庫行を追加する
        if (transform.TryGetComponent<InventoryRowUI>(out row) == false)
        {
            Debug.LogWarning("ToolbarUI: Failed to find inventory row component.");
        }

        // Add the selection outline
        // 選択範囲のアウトラインを追加する
        Transform outlineTransform = transform.Find("Selection Outline");
        if (outlineTransform.TryGetComponent<SelectionOutlineUI>(out slnOutline) == false)
        {
            Debug.LogWarning("ToolbarUI: Failed to find selection outline component.");
        }
    }
#endif
}
