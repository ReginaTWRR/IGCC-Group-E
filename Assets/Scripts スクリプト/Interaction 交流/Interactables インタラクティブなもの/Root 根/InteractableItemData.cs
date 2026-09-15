using UnityEngine;

public abstract class InteractableItemData : ItemData
{
    [Header("Interactable Item")]
    public ItemEffect effect;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (effect == null)
        {
            Debug.LogWarning("InteractableItemData: Please assign an effect.");
        }
    }
#endif
}
