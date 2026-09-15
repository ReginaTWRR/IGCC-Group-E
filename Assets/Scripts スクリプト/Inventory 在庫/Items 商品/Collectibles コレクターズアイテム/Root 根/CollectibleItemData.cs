using UnityEngine;

public abstract class CollectibleItemData : ItemData
{
    [Header("Collectible Item")]
    public Sprite itemSprite;

    [Header("Usability")]
    public ItemEffect effect;
    public bool useOnCollection = false;
    public bool IsUsable => (effect != null);

    [Header("Tooltip")]
    public bool hasTooltip = true;
    [TextArea(2, 5)] public string tooltipDescription;

    [Header("Consumability")]
    public int consumptionPerUse = 1;
    public bool IsConsumable => (consumptionPerUse > 0);
}
