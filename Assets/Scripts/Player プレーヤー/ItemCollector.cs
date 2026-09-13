using UnityEngine;

public class ItemCollector : PersistentSingleton<ItemCollector>
{
    [Header("Item Collector")]
    [SerializeField] PointEffector3D playerMagnet;

    public void CollectItem(CollectibleItemInstance item, GameObject obj)
    {
        Inventory.Instance.CollectItem(item);
        playerMagnet.RemoveObject(obj);
    }
}
