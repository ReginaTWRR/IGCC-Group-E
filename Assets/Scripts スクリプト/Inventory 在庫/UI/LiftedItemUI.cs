using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class LiftedItemUI : MonoBehaviour
{
    [Header("Lifted Item")]
    public Image itemImage;
    public TextMeshProUGUI quantityText;

    [System.NonSerialized] public CollectibleItemData item = null;
    [System.NonSerialized] public int currentQuantity = 0;

    public bool IsLiftingItem => (item != null);

    private void Awake()
    {
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLiftingItem)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }

    public void LiftItem(CollectibleItemData item, int currentQuantity)
    {
        this.item = item;
        this.currentQuantity = currentQuantity;
        UpdateUI();
    }

    public void PlaceItem()
    {
        item = null;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (IsLiftingItem)
        {
            itemImage.sprite = item.itemSprite;
            quantityText.text = currentQuantity.ToString();
        }
        else
        {
            itemImage.sprite = null;
            quantityText.text = "0";
        }

        itemImage.enabled = IsLiftingItem;
        quantityText.enabled = IsLiftingItem;
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
