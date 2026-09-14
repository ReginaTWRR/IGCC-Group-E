using UnityEngine;
using System.Collections.Generic;

public class Inventory : PersistentSingleton<Inventory>
{
    [Header("Inventory")]
    [SerializeField] CollectibleItemDatabase collectibleDB;
    [SerializeField] int capacity = 36;

    public CollectibleItemDatabase CollectibleDB => collectibleDB;

    List<InventorySlot> slots = new();
    public List<InventorySlot> Slots => slots;

    protected override void Awake()
    {
        base.Awake();

        // Initialise slots
        // スロットを初期化する
        for (int i = 0; i < capacity; ++i)
        {
            InventorySlot newSlot = new() { index = i };
            slots.Add(newSlot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player used an item
        // プレイヤーがアイテムを使用したかどうかを確認する
        if (InventoryInputHandler.Instance.CheckUseItemPressed())
        {
            UseItem();
        }
    }

    public bool CollectItem(CollectibleItemInstance item)
    {
        // Find a free slot in the list and assign the item to it
        // リスト内の空きスロットを見つけて、そこにアイテムを割り当てる
        if (FindUnoccupiedSlot(out InventorySlot unoccupiedSlot) == false)
        {
            Debug.LogWarning("Inventory: Inventory is full.");
            return false;
        }

        // Assign the item to the unoccupied slot
        // アイテムを空いているスロットに割り当てる
        unoccupiedSlot.AssignItem(item.data, 0);

        // Update the InventorySlot
        // InventorySlotを更新する
        unoccupiedSlot.AddToStack();

        // Update the UI
        // UIを更新する
        InventoryUI.Instance.UpdateUI(unoccupiedSlot.index);

        return true;
    }

    public void ClearSlotAtIndex(int index)
    {
        // Remove the item at this slot
        // このスロットにあるアイテムを削除します
        InventorySlot slot = FindSlotByIndex(index);
        slot.Clear();
    }

    public void AssignSlotAtIndex(CollectibleItemData item, int currentQuantity, int index)
    {
        // Assign an item to this slot
        // このスロットにアイテムを割り当てます
        InventorySlot slot = FindSlotByIndex(index);
        slot.AssignItem(item, currentQuantity);
    }

    private void UseItem()
    {
        // Get the selected slot and check if there is an item there
        // 選択されたスロットを取得し、そこにアイテムがあるかどうかを確認します
        InventorySlot selectedSlot = ToolbarUI.Instance.GetSelectedSlot();
        if (selectedSlot.IsOccupied == false) return;

        // Use the item
        // アイテムを使用する
        if (selectedSlot.UseItem())
        {
            // Update the UI if necessary
            // 必要に応じてUIを更新する
            InventoryUI.Instance.UpdateUI(selectedSlot.index);
        }
    }

    private bool FindUnoccupiedSlot(out InventorySlot unoccupiedSlot)
    {
        unoccupiedSlot = null;

        // Go through the slots and look for an unoccupied one
        // スロットを順番に見て、空いているスロットを探す
        foreach (InventorySlot slot in slots)
        {
            if (slot.IsOccupied == false)
            {
                unoccupiedSlot = slot;
                return true;
            }
        }

        return false;
    }

    private InventorySlot FindSlotByIndex(int index)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.index == index) return slot;
        }

        return null;
    }
}
