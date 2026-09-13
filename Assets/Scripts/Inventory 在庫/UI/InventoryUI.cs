using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : PersistentSingleton<InventoryUI>
{
    [Header("Inventory")]
    [SerializeField] InventoryRowUI toolbarRow;
    [SerializeField] List<InventoryRowUI> inventoryRows;
    [SerializeField] LiftedItemUI liftedItem;
    [SerializeField] bool isInventoryOpen = false;

    public bool IsInventoryOpen => isInventoryOpen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Assign the inventory slots
        // インベントリスロットを割り当てる
        AssignInventorySlots();

        // Update the UI of each inventory row
        // 各在庫行のUIを更新する
        toolbarRow.UpdateUI();

        foreach (InventoryRowUI inventoryRow in inventoryRows)
        {
            inventoryRow.UpdateUI();
            inventoryRow.gameObject.SetActive(isInventoryOpen);
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
        // Look for the InventorySlotUI that we should update in the toolbar first
        // まずツールバーで更新するInventorySlotUIを探します
        foreach (InventorySlotUI slot in toolbarRow.slots)
        {
            if (slot.slot.index == updatedSlotIndex)
            {
                slot.UpdateUI();
                return;
            }
        }

        // Look for the InventorySlotUI that we should update in the rest of the inventory rows
        // 残りの在庫行で更新する必要のある InventorySlotUI を探します
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

        // Assign the toolbar first
        // まずツールバーを割り当てます
        index = toolbarRow.AssignSlots(slots, index);

        // Assign each inventory row
        // 各在庫行を割り当てる
        foreach (InventoryRowUI inventoryRow in inventoryRows)
        {
            index = inventoryRow.AssignSlots(slots, index);
        }
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        // Update the visibility of each inventory row accordingly
        // 各在庫行の表示設定を適宜更新します
        foreach (InventoryRowUI inventoryRow in inventoryRows)
        {
            inventoryRow.gameObject.SetActive(IsInventoryOpen);
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
            Transform inventoryTransform = transform.GetChild(i);

            if (inventoryTransform.TryGetComponent<InventoryRowUI>(out InventoryRowUI inventoryRow) == false)
            {
                Debug.LogWarning("InventoryUI: Failed to find inventory row components.");
                return;
            }

            inventoryRows.Add(inventoryRow);
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
