[System.Serializable]
public class InventorySlot
{
    // Each inventory slot owns an item instance
    // 各インベントリスロットはアイテムインスタンスを所有します

    public CollectibleItemData item;
    public int currentQuantity = 0;

    public bool IsOccupied => (item != null);

    public void AssignItem(CollectibleItemData item)
    {
        this.item = item;
    }

    public void AddToStack(int quantity = 1)
    {
        currentQuantity += quantity;
    }
}
