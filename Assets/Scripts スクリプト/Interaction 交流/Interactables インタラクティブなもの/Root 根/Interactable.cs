using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [Header("Interactable")]
    [SerializeField] protected InteractableItemInstance instance;

    protected abstract void CheckInteraction();

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckInteraction();
    }

    protected void Interact()
    {
        // Call the item's OnInteract() function
        // アイテムの OnInteract() 関数を呼び出す
    }
}
