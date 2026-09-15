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
        // Call the item's effect function
        // アイテムのエフェクト関数を呼び出す
        instance.data.effect.TriggerEffect();

        // Update the number of interactions
        // インタラクション数を更新する
        ++instance.currentInteractTimes;

        // Check if the item has run out of interactions
        // アイテムのインタラクション回数が上限に達したかどうかを確認する
        if (instance.IsOutOfInteractions)
        {
            // Destroy the item
            // アイテムを破棄する
            Destroy(gameObject);
        }
    }
}
