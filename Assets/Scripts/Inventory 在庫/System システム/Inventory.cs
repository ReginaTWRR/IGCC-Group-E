using UnityEngine;
using System.Collections.Generic;

public class Inventory : PersistentSingleton<Inventory>
{
    [Header("Inventory")]
    [SerializeField] int capacity = 36;

    List<InventorySlot> slots = new();
    public List<InventorySlot> Slots => slots;

    protected override void Awake()
    {
        base.Awake();

        // Initialise slots
        // スロットを初期化する
        for (int i = 0; i < capacity; ++i)
        {
            slots.Add(new InventorySlot());
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

        unoccupiedSlot.AssignItem(item.data);

        // Update the InventorySlot
        // InventorySlotを更新する
        unoccupiedSlot.AddToStack();

        // In this system, the UI is "rebuilt" after every update
        // このシステムでは、UIは更新のたびに「再構築」されます
        InventoryUI.Instance.BuildUI();

        return true;
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
}
