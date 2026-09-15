using UnityEngine;

[CreateAssetMenu(fileName = "ClickableItemData", menuName = "Scriptable Objects/Inventory/Items/Collectibles/ClickableItemData")]
public class ClickableItemData : CollectibleItemData
{
    [Header("Glow Effect")]
    public float glowIntensity = 4f;
}
