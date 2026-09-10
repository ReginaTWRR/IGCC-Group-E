using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipUI : PersistentSingleton<TooltipUI>
{
    [Header("Tooltip")]
    [SerializeField] Image tooltipImage;
    [SerializeField] TextMeshProUGUI descriptionText;

    bool isTooltipShown = false;

    protected override void Awake()
    {
        base.Awake();

        UpdateUI();
    }

    public void ShowTooltip()
    {
        isTooltipShown = true;
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
