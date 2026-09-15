using UnityEngine;

[CreateAssetMenu(fileName = "HealthItemEffect", menuName = "Scriptable Objects/Items/Effects/HealthItemEffect")]
public class HealthItemEffect : ItemEffect
{
    [Header("Health Item")]
    [SerializeField] float healAmount = 0f;

    public override void TriggerEffect()
    {
        Player.Instance.Heal(healAmount);
    }
}
