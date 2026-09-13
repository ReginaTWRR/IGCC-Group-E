using UnityEngine;

[CreateAssetMenu(fileName = "ColaItemEffect", menuName = "Scriptable Objects/Inventory/Items/Effects/ColaItemEffect")]
public class ColaItemEffect : TimedItemEffect
{
    [Header("Cola")]
    public float speedUpRate = 0.15f;

    public override void Use()
    {
        base.Use();

        Player.Instance.SpeedUp(speedUpRate, effectDuration);
    }
}
