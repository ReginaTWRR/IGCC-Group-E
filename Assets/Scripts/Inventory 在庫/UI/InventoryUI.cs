using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] GameObject toolbarRow;
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
        toolbarRow = transform.Find("Toolbar Row").gameObject;

        if (toolbarRow == null)
        {
            Debug.LogWarning("InventoryUI: Failed to find toolbar row.");
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
