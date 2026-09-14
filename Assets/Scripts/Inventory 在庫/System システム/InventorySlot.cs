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

    public void Clear()
    {
        item = null;
        currentQuantity = 0;
    }
}
