using UnityEngine;

[CreateAssetMenu(fileName = "ColaItemEffect", menuName = "Scriptable Objects/Inventory/Items/Effects/ColaItemEffect")]
public class ColaItemEffect : ItemEffect
{
    [Header("Cola")]
    [SerializeField] float speedUpRate = 0.15f;
    [SerializeField] float speedUpDuration = 20f;

    public override void Use()
    {
        Player.Instance.SpeedUp(speedUpRate, speedUpDuration);
    }
}
