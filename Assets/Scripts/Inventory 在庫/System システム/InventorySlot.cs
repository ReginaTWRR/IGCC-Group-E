public class InventorySlot
{
    // Each inventory slot owns an item instance
    // 各インベントリスロットはアイテムインスタンスを所有します

    ItemInstance item;
    public bool IsOccupied => (item != null);

    public void AssignItem(ItemInstance item)
    {
        this.item = item;
    }
}
