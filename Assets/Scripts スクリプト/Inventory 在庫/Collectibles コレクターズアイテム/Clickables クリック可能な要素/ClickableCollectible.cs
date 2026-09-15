using UnityEngine;

public class ClickableCollectible : Collectible
{
    [Header("Clickable Collectible")]
    [SerializeField] GlowOnMouseHover glowEffect;

    protected override void CheckCollection()
    {
        // Check if the item has been clicked while the mouse is hovered over it
        // マウスカーソルがアイテムの上に重なっている間に、アイテムがクリックされたかどうかを確認します。
        if (glowEffect.IsGlowing && InventoryInputHandler.Instance.CheckCollectItemPressed())
        {
            CollectItem();
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the glow effect
        // グロー効果を追加する
        glowEffect = null;
        
        if (TryGetComponent<GlowOnMouseHover>(out glowEffect) == false)
        {
            Debug.LogWarning("ClickableCollectible: Failed to find glow effect component.");
        }
    }
#endif
}
