using UnityEngine;

[CreateAssetMenu(fileName = "ColaItemEffect", menuName = "Scriptable Objects/Items/Effects/ColaItemEffect")]
public class ColaItemEffect : TimedItemEffect
{
    [Header("Cola")]
    public float speedUpRate = 0.15f;

    public override bool TriggerEffect()
    {
        if (base.TriggerEffect() == false) return false;

        Player.Instance.SpeedUp(speedUpRate, effectDuration);

        return true;
    }
}
