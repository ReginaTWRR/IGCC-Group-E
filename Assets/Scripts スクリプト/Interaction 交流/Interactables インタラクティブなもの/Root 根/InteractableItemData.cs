using UnityEngine;

public abstract class InteractableItemData : ItemData
{
    [Header("Interactable Item")]
    public ItemEffect effect;

    [Header("Usability")]
    // -1 is treated as an infinite / unlimited number of interactions
    // -1 は無限/無制限の相互作用として扱われます
    public int maxInteractions = -1;
    
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
