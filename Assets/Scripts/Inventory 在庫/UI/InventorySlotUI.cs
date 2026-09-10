using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Inventory Slot")]
    public Image itemImage;
    public TextMeshProUGUI quantityText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemImage.enabled == true && quantityText.enabled == true)
        {
            TooltipUI.Instance.ShowTooltip(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TooltipUI.Instance.IsTooltipShown)
        {
            TooltipUI.Instance.HideTooltip();
        }
    }

    public void SetUI(Sprite newSprite, string newText)
    {
        itemImage.sprite = newSprite;
        itemImage.enabled = true;

        quantityText.text = newText;
        quantityText.enabled = true;
    }

    public void ClearUI()
    {
        itemImage.sprite = null;
        itemImage.enabled = false;

        quantityText.text = "0";
        quantityText.enabled = false;
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the item image
        // アイテム画像を追加する
        Transform imageTransform = transform.Find("Item Image");
        if (imageTransform.TryGetComponent<Image>(out itemImage) == false)
        {
            Debug.LogWarning("InventorySlotUI: Failed to find item image object.");
        }

        // Add the quantity text
        // 数量テキストを追加
        Transform textTransform = transform.Find("Quantity Text");
        if (textTransform.TryGetComponent<TextMeshProUGUI>(out quantityText) == false)
        {
            Debug.LogWarning("InventorySlotUI: Failed to find quantity text object.");
        }
    }
#endif
}
