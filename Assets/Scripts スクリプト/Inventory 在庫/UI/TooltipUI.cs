using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipUI : PersistentSingleton<TooltipUI>
{
    [Header("Tooltip")]
    [SerializeField] Image tooltipImage;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] Vector3 localOffset;

    bool isTooltipShown = false;
    public bool IsTooltipShown => isTooltipShown;

    protected override void Awake()
    {
        base.Awake();

        UpdateUI();
    }

    public void ShowTooltip(CollectibleItemData data, Transform hoveredSlot)
    {
        isTooltipShown = true;

        // Update the description
        // 説明を更新します
        descriptionText.text = data.tooltipDescription;

        // Visually move the image
        // 画像を視覚的に移動させる
        tooltipImage.rectTransform.SetParent(hoveredSlot.transform, false);
        tooltipImage.rectTransform.localPosition = Vector3.zero + localOffset;

        UpdateUI();
    }

    public void HideTooltip()
    {
        isTooltipShown = false;
        UpdateUI();
    }

    private void UpdateUI()
    {
        tooltipImage.enabled = isTooltipShown;
        descriptionText.enabled = isTooltipShown;
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the tooltip image
        // ツールチップ画像を追加する
        tooltipImage = null;

        if (transform.TryGetComponent<Image>(out tooltipImage) == false)
        {
            Debug.LogWarning("TooltipUI: Failed to find tooltip image component.");
        }

        // Add the description text
        // 説明文を追加する
        descriptionText = null;
        Transform descriptionTransform = transform.GetChild(0);

        if (descriptionTransform.TryGetComponent<TextMeshProUGUI>(out descriptionText) == false)
        {
            Debug.LogWarning("TooltipUI: Failed to find description text component.");
        }
    }
#endif
}
