using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : PersistentSingleton<InventoryUI>
{
    [Header("Inventory")]
    [SerializeField] InventoryRowUI toolbarRow;
    [SerializeField] List<GameObject> inventoryRows;
    [SerializeField] LiftedItemUI liftedItem;

    bool isInventoryOpen = false;
    public bool IsInventoryOpen => isInventoryOpen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Assign the inventory slots
        // インベントリスロットを割り当てる
        List<InventorySlot> slots = Inventory.Instance.Slots;

        for (int i = 0; i < slots.Count; ++i)
        {
            if (i <= 11)
            {
                toolbarRow.slots[i].slot = slots[i];
            }
        }

        // Update the UI of each inventory slot
        // 各インベントリスロットのUIを更新する
        foreach (InventorySlotUI slot in toolbarRow.slots)
        {
            slot.UpdateUI();
        }

        // Hide the inventory rows
        // 最初に在庫行を非表示にする
        foreach (GameObject inventoryRow in inventoryRows)
        {
            inventoryRow.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (InventoryInputHandler.Instance.CheckToggleInventoryPressed())
        {
            ToggleInventory();
        }
    }

    public void UpdateUI(int updatedSlotIndex)
    {
        // Look for the InventorySlotUI that we should update
        // 更新すべき InventorySlotUI を探します
        foreach (InventorySlotUI slot in toolbarRow.slots)
        {
            if (slot.slot.index == updatedSlotIndex)
            {
                slot.UpdateUI();
                return;
            }
        }
    }

    public void OnInventorySlotClicked(InventorySlotUI slotClicked)
    {
        if (liftedItem.IsLiftingItem == false)
        {
            if (slotClicked.IsOccupied)
            {
                // Lift the item
                // アイテムを持ち上げる
                liftedItem.LiftItem(slotClicked.slot.item, slotClicked.slot.currentQuantity);
                Inventory.Instance.ClearSlotAtIndex(slotClicked.slot.index);
            }
        }
        else
        {
            if (slotClicked.IsOccupied == false)
            {
                // Place the item
                // アイテムを配置する
                Inventory.Instance.AssignSlotAtIndex(liftedItem.item, liftedItem.currentQuantity, slotClicked.slot.index);
                liftedItem.PlaceItem();
            }
        }

        slotClicked.UpdateUI();
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        foreach (GameObject inventoryRow in inventoryRows)
        {
            inventoryRow.SetActive(isInventoryOpen);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the toolbar row
        // ツールバー行を追加する
        toolbarRow = null;
        Transform toolbarTransform = transform.Find("Toolbar Row");

        if (toolbarTransform.TryGetComponent<InventoryRowUI>(out toolbarRow) == false)
        {
            Debug.LogWarning("InventoryUI: Failed to find toolbar row component.");
        }

        // Add the inventory rows
        // 在庫行を追加する
        inventoryRows.Clear();

        for (int i = 0; i < 2; ++i)
        {
            Transform currentChild = transform.GetChild(i);
            inventoryRows.Add(currentChild.gameObject);
        }

        // Add the lifted item
        // 持ち上げたアイテムを追加する
        liftedItem = null;
        Transform liftedItemTransform = transform.Find("Lifted Item");

        if (liftedItemTransform.TryGetComponent<LiftedItemUI>(out liftedItem) == false)
        {
            Debug.LogWarning("InventoryUI: Failed to find lifted item component.");
        }
    }
#endif
}
