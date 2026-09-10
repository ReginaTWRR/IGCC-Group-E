using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible")]
    [SerializeField] CollectibleItemInstance instance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ItemCollector>(out ItemCollector collector))
        {
            collector.CollectItem(instance, gameObject);
            Destroy(gameObject);
        }
    }
}
