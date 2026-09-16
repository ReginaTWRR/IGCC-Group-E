using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Battle/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;

    [Header("HP")]
    public int maxHp;
    public int currentHp;

    [Header("Combat")]
    public int baseAttackPower;
    [Range(0, 100)] public int baseHitRate = 90;

    [Header("Special Gauge (%)")]
    public int maxSpecialGauge = 400;
    public int currentSpecialGauge = 0;

    // 現在かかっている状態異常のリスト
    [SerializeField] public List<ActiveEffect> activeEffects = new List<ActiveEffect>();

    public bool IsDead => currentHp <= 0;
    public bool IsSpecialReady => currentSpecialGauge >= maxSpecialGauge;

    public int GetCurrentAttack()
    {
        bool hasDebuff = activeEffects.Exists(e => e.type == EffectType.Debuff);
        return hasDebuff ? Mathf.Max(1, baseAttackPower / 2) : baseAttackPower;
    }

    // 現在の状態異常を考慮した命中率を取得
    public int GetCurrentHitRate()
    {
        bool hasBlind = activeEffects.Exists(e => e.type == EffectType.Blind);
        return hasBlind ? baseHitRate / 2 : baseHitRate;
    }

    public bool HasDebuff() => activeEffects.Exists(e => e.type == EffectType.Debuff);
    public int HasDebuffTurn()
    {
        var targetEffect = activeEffects.Find(e => e.type == EffectType.Debuff);

        return targetEffect.durationTurns;

    }
   
    public bool HasPoison() => activeEffects.Exists(e => e.type == EffectType.Poison);

    public int HasPoisonTurn()
    {
        var targetEffect = activeEffects.Find(e => e.type == EffectType.Poison);

        return targetEffect.durationTurns;

    }
    public bool HasReflect() => activeEffects.Exists(e => e.type == EffectType.Reflect);
    public bool HasBlind() => activeEffects.Exists(e => e.type == EffectType.Blind);

    public int HasBlindTurn()
    {
        var targetEffect = activeEffects.Find(e => e.type == EffectType.Blind);

        return targetEffect.durationTurns;

    }
    public bool HasShield() => activeEffects.Exists(e => e.type == EffectType.Shield);
    
    
    

    public void TakeDamage(int damage)
    {
        currentHp = Mathf.Clamp(currentHp - damage, 0, maxHp);
    }

    public void ChargeSpecialGauge(int amount)
    {
        currentSpecialGauge = Mathf.Clamp(currentSpecialGauge + amount, 0, maxSpecialGauge);
    }

    public bool EvaluateHit()
    {
        return Random.Range(0, 100) < GetCurrentHitRate();
    }

    // 状態異常の付与
    public void AddEffect(EffectType type, int turns)
    {
        // 既存の同じ効果があればターン数を上書き、なければ新規追加
        int index = activeEffects.FindIndex(e => e.type == type);
        if (index >= 0)
        {
            var effect = activeEffects[index];
            effect.durationTurns = turns;
            activeEffects[index] = effect;
        }
        else
        {
            activeEffects.Add(new ActiveEffect { type = type, durationTurns = turns });
        }
    }

    // シールドや反射などの「1回消費型」の効果を消去
    public void RemoveEffect(EffectType type)
    {
        activeEffects.RemoveAll(e => e.type == type);
    }
}
