using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleItemData", menuName = "Scriptable Objects/Inventory/Items/Collectibles/CollectibleItemData")]
public class CollectibleItemData : ItemData
{
    [Header("Collectible Item Data")]
    public Sprite itemSprite;

    [Header("Tooltip")]
    public bool hasTooltip = true;
    [TextArea(2, 5)] public string tooltipDescription;

    [Header("Consumable")]
    public bool isConsumable = true;
    public int consumptionPerUse = 1;

    public bool IsUsable => (effect != null);
}
