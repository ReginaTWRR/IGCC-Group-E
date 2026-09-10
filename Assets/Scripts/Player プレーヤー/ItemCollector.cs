using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [Header("Item Collector")]
    [SerializeField] PointEffector3D playerMagnet;

    public void CollectItem(ItemInstance item, GameObject obj)
    {
        Inventory.Instance.CollectItem(item);
        playerMagnet.RemoveObject(obj);
    }
}
