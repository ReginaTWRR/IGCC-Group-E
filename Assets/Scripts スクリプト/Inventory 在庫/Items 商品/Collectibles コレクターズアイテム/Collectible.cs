using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible")]
    [SerializeField] CollectibleItemInstance instance;
    [SerializeField] HoverEffect3D hoverEffect;
    [SerializeField] float maxCollectionDuration = 1f;

    float attractionTime = -Mathf.Infinity;
    bool isBeingCollected = false;

    // Update is called once per frame
    void Update()
    {
        // Check if the item has been following the player for too long
        // アイテムがプレイヤーに追従しすぎているかどうかを確認する
        if (isBeingCollected)
        {
            if ((Time.time - attractionTime) >= maxCollectionDuration)
            {
                CollectItem();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Magnet"))
        {
            hoverEffect.StopHovering();
            attractionTime = Time.time;
            isBeingCollected = true;
        }
        else if (other.CompareTag("Player"))
        {
            CollectItem();
        }
    }

    private void CollectItem()
    {
        PlayerItemCollector.Instance.CollectItem(instance, gameObject);
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Assign the instance
        // インスタンスを割り当てる
        instance = null;

        if (TryGetComponent<CollectibleItemInstance>(out instance) == false)
        {
            Debug.LogWarning("Collectible: Failed to find collectible item instance component.");
        }

        // Assign the hoverEffect
        // ホバーエフェクトを割り当てる
        hoverEffect = null;

        if (TryGetComponent<HoverEffect3D>(out hoverEffect) == false)
        {
            Debug.LogWarning("Collectible: Failed to find hover effect component.");
        }
    }
#endif
}
