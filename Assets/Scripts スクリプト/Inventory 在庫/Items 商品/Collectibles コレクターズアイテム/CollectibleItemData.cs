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
    public int consumptionPerUse = 1;
    public bool IsConsumable => (consumptionPerUse > 0);
}
