using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ItemCollector>(out ItemCollector collector))
        {
            collector.CollectItem(gameObject);
            Destroy(gameObject);
        }
    }
}
