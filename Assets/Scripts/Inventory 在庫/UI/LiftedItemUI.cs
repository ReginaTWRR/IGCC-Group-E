using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LiftedItemUI : MonoBehaviour
{
    [Header("Lifted Item")]
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI quantityText;

    bool isUIEnabled = false;

    private void Awake()
    {
        UpdateUI();
    }

    public void LiftItem()
    {
        isUIEnabled = true;
        UpdateUI();
    }

    private void UpdateUI()
    {
        itemImage.enabled = isUIEnabled;
        quantityText.enabled = isUIEnabled;
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the item image
        // アイテム画像を追加する
        itemImage = null;
        Transform imageTransform = transform.Find("Item Image");

        if (imageTransform.TryGetComponent<Image>(out itemImage) == false)
        {
            Debug.LogWarning("LifedItemUI: Failed to find item image component.");
        }

        // Add the quantity text
        // 数量テキストを追加
        quantityText = null;
        Transform textTransform = transform.Find("Quantity Text");

        if (textTransform.TryGetComponent<TextMeshProUGUI>(out quantityText) == false)
        {
            Debug.LogWarning("LiftedItemUI: Failed to find quantity text component.");
        }
    }
#endif
}
