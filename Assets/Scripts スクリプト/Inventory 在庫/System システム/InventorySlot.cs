[System.Serializable]
public class InventorySlot
{
    // Each inventory slot owns an item instance
    // 各インベントリスロットはアイテムインスタンスを所有します

    public CollectibleItemData item;
    public int currentQuantity = 0;
    public int index;

    public bool IsOccupied => (item != null);

    public void AssignItem(CollectibleItemData item, int currentQuantity)
    {
        this.item = item;
        this.currentQuantity = currentQuantity;
    }

    public void AddToStack(int quantity = 1)
    {
        currentQuantity += quantity;
    }

    public bool UseItem()
    {
        // Check if there is an item to use and if it is usable
        // 使用できるアイテムが存在するか、また使用可能かどうかを確認する
        if (IsOccupied == false || item.IsUsable == false) return false;

        // Use the item
        // アイテムを使用する
        item.effect.TriggerEffect();

        // Consume the item where applicable
        // 該当する場合はアイテムを消費する
        if (item.IsConsumable)
        {
            currentQuantity -= item.consumptionPerUse;

            if (currentQuantity <= 0)
            {
                Clear();
            }
        }

        return true;
    }

    public void Clear()
    {
        item = null;
        currentQuantity = 0;
    }
}
