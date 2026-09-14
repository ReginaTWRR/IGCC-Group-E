using UnityEngine;

[CreateAssetMenu(fileName = "ClickableItemData", menuName = "Scriptable Objects/Inventory/Items/Clickables/ClickableItemData")]
public class ClickableItemData : CollectibleItemData
{
    [Header("Clickable Item")]
    public float glowIntensity = 4f;
}
