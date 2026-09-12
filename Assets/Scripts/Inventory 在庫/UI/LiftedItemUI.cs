using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class LiftedItemUI : MonoBehaviour
{
    [Header("Lifted Item")]
    public Image itemImage;
    public TextMeshProUGUI quantityText;

    bool isLiftingItem = false;
    public bool IsLiftingItem => isLiftingItem;

    private void Awake()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLiftingItem)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }

    public void LiftItem(Sprite newSprite, string newText)
    {
        isLiftingItem = true;
        UpdateUI(newSprite, newText);
    }

    public void PlaceItem()
    {
        isLiftingItem = false;
        UpdateUI();
    }

    private void UpdateUI(Sprite newSprite = null, string newText = "0")
    {
        itemImage.sprite = newSprite;
        itemImage.enabled = isLiftingItem;

        quantityText.text = newText;
        quantityText.enabled = isLiftingItem;
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
