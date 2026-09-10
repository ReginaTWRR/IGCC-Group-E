using UnityEngine;

public class ItemInstance : MonoBehaviour
{
    [Header("Item")]
    public int currentQuantity;

    public void AddToStack(int quantity = 1)
    {
        currentQuantity += quantity;
    }
}
