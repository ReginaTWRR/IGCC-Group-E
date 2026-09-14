using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleItemData", menuName = "Scriptable Objects/Inventory/Items/Collectibles/CollectibleItemData")]
public class CollectibleItemData : ItemData
{
    [Header("Collectible Item")]
    public ItemEffect effect;
    public Sprite itemSprite;
    public bool IsUsable => (effect != null);

    [Header("Tooltip")]
    public bool hasTooltip = true;
    [TextArea(2, 5)] public string tooltipDescription;

    [Header("Consumability")]
    public int consumptionPerUse = 1;
    public bool IsConsumable => (consumptionPerUse > 0);
}
