using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : PersistentSingleton<InventoryUI>
{
    [Header("Inventory")]
    [SerializeField] InventoryRowUI toolbarRow;
    [SerializeField] List<GameObject> inventoryRows;

    bool isInventoryOpen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Hide the inventory rows at the start
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

    public void BuildUI()
    {
        // Clear the UI first
        // まずUIをクリアする
        ClearUI();

        // Build the UI according to the list of inventory slots
        // インベントリスロットのリストに基づいてUIを構築する
        List<InventorySlot> slots = Inventory.Instance.Slots;

        for (int i = 0; i < slots.Count; ++i)
        {
            if (slots[i].IsOccupied == false) continue;

            if (i <= 11)
            {
                // This slot will be displayed at the toolbar
                // このスロットはツールバーに表示されます
            }
        }
    }

    private void ClearUI()
    {
        // Loop through each slot and clear the item image and text
        // 各スロットをループ処理し、アイテムの画像とテキストをクリアする
        foreach (InventorySlotUI slot in toolbarRow.slots)
        {
            slot.ClearUI();
        }
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

        int iterator = 1;
        while (true)
        {
            Transform currentChild = transform.GetChild(iterator++);
            inventoryRows.Add(currentChild.gameObject);

            if (iterator > transform.childCount - 1) break;
        }
    }
#endif
}
