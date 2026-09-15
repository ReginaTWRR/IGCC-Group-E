using UnityEngine;

public class ClickableInteractable : Interactable
{
    [Header("Clickable Interactable")]
    [SerializeField] GlowOnMouseHover glowEffect;

    protected override void CheckInteraction()
    {
        // Check if the item has been clicked while the mouse is hovered over it
        // マウスカーソルがアイテムの上に重なっている間に、アイテムがクリックされたかどうかを確認します。
        if (glowEffect.IsGlowing && InteractionInputHandler.Instance.CheckClickInteractPressed())
        {
            Interact();
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
            Debug.LogWarning("ClickableInteractable: Failed to find glow effect component.");
        }
    }
#endif
}
