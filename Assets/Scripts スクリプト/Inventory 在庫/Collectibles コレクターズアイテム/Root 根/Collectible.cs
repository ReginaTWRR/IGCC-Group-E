using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    [Header("Collectible")]
    [SerializeField] protected CollectibleItemInstance instance;

    protected abstract void CheckCollection();

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckCollection();
    }

    protected void CollectItem()
    {
        // Collect the item
        // アイテムを収集する
        PlayerItemCollector.Instance.CollectItem(instance, gameObject);

        // Use the item where applicable
        // 該当する場合はアイテムを使用してください
        if (instance.data.IsUsable && instance.data.useOnCollection)
        {
            instance.data.effect.TriggerEffect();
        }

        // Destroy the item
        // アイテムを破棄する
        Destroy(gameObject);
    }
}
