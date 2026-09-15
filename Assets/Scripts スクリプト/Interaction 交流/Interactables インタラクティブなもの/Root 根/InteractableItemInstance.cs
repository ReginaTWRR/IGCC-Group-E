using UnityEngine;

public abstract class InteractableItemInstance : ItemInstance
{
    [Header("Interactable Item")]
    public InteractableItemData data;
    // Store the number of times that we have interacted with this item
    // このアイテムとのやり取りの回数を保存します
    [System.NonSerialized] public int currentInteractTimes = 0;

    public bool IsOutOfInteractions
    {
        get
        {
            if (data.maxInteractions == -1) return false;
            return (currentInteractTimes <= 0);
        }
    }
}
