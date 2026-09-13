using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : PersistentSingleton<InventoryUI>
{
    [Header("Inventory")]
    [SerializeField] List<InventoryRowUI> inventoryRows;
    [SerializeField] LiftedItemUI liftedItem;

    bool isInventoryOpen = false;
    public bool IsInventoryOpen => isInventoryOpen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Assign the inventory slots
        // インベントリスロットを割り当てる
        AssignInventorySlots();

        // Update the UI of each inventory row
        // 各在庫行のUIを更新する
        foreach (InventoryRowUI inventoryRow in inventoryRows)
        {
            inventoryRow.UpdateUI();
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
        // Look for the InventorySlotUI that we should update in each inventory row
        // 各在庫行で更新すべき InventorySlotUI を探します
        foreach (InventoryRowUI inventoryRow in inventoryRows)
        {
            foreach (InventorySlotUI slot in inventoryRow.slots)
            {
                if (slot.slot.index == updatedSlotIndex)
                {
                    slot.UpdateUI();
                    return;
                }
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

    private void AssignInventorySlots()
    {
        // Get the inventory slots
        // インベントリスロットを取得する
        List<InventorySlot> slots = Inventory.Instance.Slots;
        int index = 0;

        // Assign each inventory row
        // 各在庫行を割り当てる
        foreach (InventoryRowUI inventoryRow in inventoryRows)
        {
            index = inventoryRow.AssignSlots(slots, index);
        }
    }

    private void ToggleInventory()
    {
        // Update the visibility of each inventory row accordingly
        // 各在庫行の表示設定を適宜更新します
        foreach (InventoryRowUI inventoryRow in inventoryRows)
        {
            inventoryRow.ToggleVisibility();
        }

        // Update the boolean for other systems
        // 他のシステム用にブール値を更新する
        isInventoryOpen = !isInventoryOpen;
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the toolbar row first
        // まずツールバー行を追加します
        inventoryRows.Clear();

        Transform toolbarTransform = transform.Find("Toolbar Row");

        if (toolbarTransform.TryGetComponent<InventoryRowUI>(out InventoryRowUI toolbarRow))
        {
            inventoryRows.Add(toolbarRow);
        }
        else
        {
            Debug.LogWarning("InventoryUI: Failed to find toolbar row component.");
        }

        // Add the inventory rows
        // 在庫行を追加する
        for (int i = 0; i < 2; ++i)
        {
            Transform inventoryTransform = transform.GetChild(i);

            if (inventoryTransform.TryGetComponent<InventoryRowUI>(out InventoryRowUI inventoryRow))
            {
                inventoryRows.Add(inventoryRow);
            }
            else
            {
                Debug.LogWarning("InventoryUI: Failed to find inventory row components.");
            }
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
