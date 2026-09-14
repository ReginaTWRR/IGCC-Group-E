using UnityEngine;

// 状態異常の種類
public enum EffectType
{
    None,
    Debuff,       // 悲しみ：攻撃力半減
    Poison,       // 自然  ：最大HP依存の持続ダメージ
    Shield,       // 宝石  ：次の攻撃を1回だけ無効化
    Reflect,      // 悪    ：受けたダメージをそのまま相手に返す
    Blind         // 光    ：相手の次の攻撃の命中率を半減
}

[System.Serializable]
public struct ActiveEffect
{
    public EffectType type;
    public int durationTurns; // 残り持続ターン数
}
