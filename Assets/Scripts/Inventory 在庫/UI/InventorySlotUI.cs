using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [Header("Inventory Slot")]
    public Image itemImage;
    public TextMeshProUGUI quantityText;

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
