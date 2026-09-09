using UnityEngine;
using UnityEngine.UI;

public class SelectionOutlineUI : Singleton<SelectionOutlineUI>
{
    [Header("Selection Outline")]
    [SerializeField] Image outlineImage;

    InventorySlotUI selectedSlot;
    public InventorySlotUI SelectedSlot => selectedSlot;

    public void MoveSelectionOutline(InventorySlotUI newSlot)
    {
        // Visually move the image
        // 画像を視覚的に移動させる
        outlineImage.rectTransform.SetParent(newSlot.transform, false);
        outlineImage.rectTransform.localPosition = Vector3.zero;

        // Update the reference
        // 参照を更新する
        selectedSlot = newSlot;
    }
}
