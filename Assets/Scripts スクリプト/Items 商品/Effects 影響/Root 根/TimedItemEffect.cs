using UnityEngine;

public abstract class TimedItemEffect : ItemEffect
{
    [Header("Timed Item")]
    public float effectDuration = 1f;

    public override bool TriggerEffect()
    {
        TimerUI.Instance.SetTimer(effectDuration);
        return true;
    }
}
