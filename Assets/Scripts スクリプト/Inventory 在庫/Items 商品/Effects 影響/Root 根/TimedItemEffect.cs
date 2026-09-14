using UnityEngine;

public abstract class TimedItemEffect : ItemEffect
{
    [Header("Timed Item")]
    public float effectDuration = 1f;

    public override void Use()
    {
        TimerUI.Instance.SetTimer(effectDuration);
    }
}
