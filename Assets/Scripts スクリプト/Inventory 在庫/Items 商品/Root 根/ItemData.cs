using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    // ItemData stores common item information that can be applied to every instance of an item.
    // ItemData には、アイテムのすべてのインスタンスに適用できる共通のアイテム情報が格納されます。

    // ItemInstance stores runtime data that is specific to its particular instance.
    // ItemInstance は、そのインスタンス固有の実行時データを格納します。

    [Header("Item Data")]
    public string itemName;
}
