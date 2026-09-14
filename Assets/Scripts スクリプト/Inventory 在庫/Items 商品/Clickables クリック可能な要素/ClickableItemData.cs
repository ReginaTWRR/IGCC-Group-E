using UnityEngine;

[CreateAssetMenu(fileName = "ClickableItemData", menuName = "Scriptable Objects/Inventory/Items/Clickables/ClickableItemData")]
public class ClickableItemData : CollectibleItemData
{
    [Header("On Collection")]
    public bool useOnCollection = false;

    [Header("Glow Effect")]
    public float glowIntensity = 4f;
}
