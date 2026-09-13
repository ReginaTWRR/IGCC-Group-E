using UnityEngine;
using System.Collections.Generic;

public class InventoryRowUI : MonoBehaviour
{
    [Header("Inventory Row")]
    // Keep a list of all the slots in the row
    // 行内のすべてのスロットのリストを保持する
    public List<InventorySlotUI> slots = new();

    private void Awake()
    {
        // Check if the row has at least one inventory slot
        // ツールバーに少なくとも1つのインベントリスロットがあるかどうかを確認します
        if (slots.Count <= 0)
        {
            Debug.LogError("InventoryRowUI: Row does not have any inventory slots.");
        }
    }

    public int AssignSlots(List<InventorySlot> invSlots, int startIndex)
    {
        for (int i = 0; i < slots.Count; ++i)
        {
            int invIndex = startIndex + i;
            if (invIndex >= invSlots.Count) return invIndex;

            slots[i].slot = invSlots[invIndex];
        }

        return startIndex + slots.Count;
    }

    public void UpdateUI()
    {
        foreach (InventorySlotUI slot in slots)
        {
            slot.UpdateUI();
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the inventory slots
        // インベントリスロットを追加する
        slots.Clear();
        Transform slotParent = transform.Find("Slots");

        for (int i = 0; i < slotParent.childCount; ++i)
        {
            Transform slotChild = slotParent.GetChild(i);
            InventorySlotUI newSlot = slotChild.GetComponent<InventorySlotUI>();
            slots.Add(newSlot);
        }
    }
#endif
}
