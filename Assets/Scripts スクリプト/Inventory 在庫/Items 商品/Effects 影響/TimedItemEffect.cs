using UnityEngine;

[CreateAssetMenu(fileName = "TimedItemEffect", menuName = "Scriptable Objects/Inventory/Items/EffectsTimedItemEffect")]
public class TimedItemEffect : ItemEffect
{
    [Header("Timed Item")]
    public float effectDuration = 1f;

    public override void Use()
    {
        TimerUI.Instance.SetTimer(effectDuration);
    }
}
