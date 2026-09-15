using UnityEngine;

public enum SkillColor { Red, Blue, Green, Yellow, Purple ,Special}

[CreateAssetMenu(fileName = "NewAttackSkill", menuName = "Battle/Attack Skill")]
public class AttackSkill : ScriptableObject
{
    public string skillName;
    public SkillColor skillColor;
    [TextArea] public string description;

    
    public byte Color_R;
    public byte Color_G;
    public byte Color_B;

    public int Execute(CharacterData user, CharacterData target, out string logMessage)
    {
        logMessage = string.Empty;

        // 黄色の効果
        if (target.HasShield() && skillColor != SkillColor.Yellow && skillColor != SkillColor.Purple && skillColor != SkillColor.Special)
        {
            logMessage = $"{target.characterName} の　シールド発動!";
            target.RemoveEffect(EffectType.Shield);
            return 0;
        }

        // 紫色の効果
        if (target.HasReflect() && skillColor != SkillColor.Purple && skillColor != SkillColor.Special)
        {
            int reflectDamage = user.GetCurrentAttack();
            user.TakeDamage(reflectDamage);
            logMessage = $"{target.characterName} の　反射発動! {user.characterName} は {reflectDamage} ダメージをうけた";
            target.RemoveEffect(EffectType.Reflect);
            return 0;
        }

        // 命中判定
        if (!user.EvaluateHit())
        {
            logMessage = $"{user.characterName} は攻撃を外した";
            return 0;
        }

        int damage = user.GetCurrentAttack();

        switch (skillColor)
        {
            case SkillColor.Red: //攻撃
                target.TakeDamage(damage);
                logMessage = $"{user.characterName} の攻撃! {target.characterName} に {damage} のダメージを与えた";
                break;

            case SkillColor.Blue: //デバフ効果（3ターン攻撃力半減）
                target.TakeDamage(damage / 2);
                target.AddEffect(EffectType.Debuff, 3);
                logMessage = $"{user.characterName} は悲しんでいる {target.characterName} は攻撃力が下がった";
                break;

            case SkillColor.Green: //最大HP参照の毒(3ターン) ＋ HP吸収
                target.TakeDamage(damage);
                target.AddEffect(EffectType.Poison, 3);
                // 50%を吸収
                int drainAmount = damage / 2;
                user.currentHp = Mathf.Clamp(user.currentHp + drainAmount, 0, user.maxHp);
                logMessage = $"{user.characterName} のかみつく! {damage} ダメージを与え {drainAmount} 分吸収した";
                break;

            case SkillColor.Yellow: //シールド付与 ＋ 相手に目くらまし(2ターン)
                user.AddEffect(EffectType.Shield, 99); //次の攻撃を防ぐまで永続
                target.AddEffect(EffectType.Blind, 2);
                logMessage = $"{user.characterName} にシールド付与 {target.characterName} は目がくらんだ";
                break;

            case SkillColor.Purple: //攻撃の反射
                user.AddEffect(EffectType.Reflect, 99);
                logMessage = $"{user.characterName} 「ばーりあ」";
                break;
            case SkillColor.Special:
                target.TakeDamage(target.maxHp);
                logMessage = $"{user.characterName} の必殺技! {target.characterName} に {target.maxHp} のダメージを与えた";
                break;
        }

        return damage;
    }
}
