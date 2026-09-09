using UnityEngine;
using System.Collections.Generic;

public class ToolbarUI : MonoBehaviour
{
    [Header("Toolbar")]
    // Keep a list of all the slots in the row
    // 行内のすべてのスロットのリストを保持する
    public List<InventorySlotUI> slots = new();
    // Store a reference to the selection outline
    // 選択範囲のアウトラインへの参照を保存します
    [SerializeField] SelectionOutlineUI slnOutline;

    // Store the selected slot index
    // 選択されたスロットのインデックスを保存します
    int selectedSlotIndex = 0;

    private void Awake()
    {
        // Check if the toolbar has at least one inventory slot
        // ツールバーに少なくとも1つのインベントリスロットがあるかどうかを確認します
        if (slots.Count <= 0)
        {
            Debug.LogError("ToolbarUI: Toolbar does not have any inventory slots.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelection();
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
            if (selectedSlotIndex < slots.Count - 1) ++selectedSlotIndex;
        }
        else if (scroll == ScrollMovement.Downwards)
        {
            if (selectedSlotIndex > 0) --selectedSlotIndex;
        }

        // Move the selection outline accordingly
        // 選択範囲の輪郭をそれに応じて移動します
        slnOutline.MoveSelectionOutline(slots[selectedSlotIndex]);
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the inventory slots
        // インベントリスロットを追加する
        Transform slotParent = transform.Find("Slots");
        slots.Clear();

        for (int i = 0; i < slotParent.childCount; ++i)
        {
            Transform slotChild = slotParent.GetChild(i);
            InventorySlotUI newSlot = slotChild.GetComponent<InventorySlotUI>();
            slots.Add(newSlot);
        }

        // Add the selection outline
        // 選択範囲のアウトラインを追加する
        Transform outlineTransform = transform.Find("Selection Outline");
        if (outlineTransform.TryGetComponent<SelectionOutlineUI>(out slnOutline) == false)
        {
            Debug.LogWarning("ToolbarUI: Failed to find selection outline object.");
        }
    }
#endif
}
