using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [Header("Item Collector")]
    [SerializeField] PointEffector3D playerMagnet;

    public void CollectItem(GameObject toCollect)
    {
        playerMagnet.RemoveObject(toCollect);
    }
}
