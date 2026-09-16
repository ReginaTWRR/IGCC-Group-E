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
            logMessage = $"{target.characterName} s shield activates!";
            target.RemoveEffect(EffectType.Shield);
            return 0;
        }

        // 紫色の効果
        if (target.HasReflect() && skillColor != SkillColor.Purple && skillColor != SkillColor.Special)
        {
            int reflectDamage = user.GetCurrentAttack();
            user.TakeDamage(reflectDamage);
            logMessage = $"{target.characterName}'s reflex activation! {user.characterName} took {reflectDamage} damaga";
            target.RemoveEffect(EffectType.Reflect);
            return 0;
        }

        // 命中判定
        if (!user.EvaluateHit())
        {
            logMessage = $"{user.characterName} missed the attack.";
            return 0;
        }

        int damage = user.GetCurrentAttack();

        switch (skillColor)
        {
            case SkillColor.Red: //攻撃
                target.TakeDamage(damage);
                logMessage = $"{user.characterName}'s attack! Dealt {damage} damage.";
                break;

            case SkillColor.Blue: //デバフ効果（3ターン攻撃力半減）
                target.TakeDamage(damage / 2);
                target.AddEffect(EffectType.Debuff, 3);
                logMessage = $"{user.characterName} is sad. {target.characterName}'s attack power decreased.";
                break;

            case SkillColor.Green: //最大HP参照の毒(3ターン) ＋ HP吸収
                target.TakeDamage(damage);
                target.AddEffect(EffectType.Poison, 3);
                // 50%を吸収
                int drainAmount = damage / 2;
                user.currentHp = Mathf.Clamp(user.currentHp + drainAmount, 0, user.maxHp);
                logMessage = $"{user.characterName} bite! Dealt {damage} damage and drained  {drainAmount}";
                break;

            case SkillColor.Yellow: //シールド付与 ＋ 相手に目くらまし(2ターン)
                user.AddEffect(EffectType.Shield, 99); //次の攻撃を防ぐまで永続
                target.AddEffect(EffectType.Blind, 2);
                logMessage = $"Granted a shield to {user.characterName}. {target.characterName} is blinded.";
                break;

            case SkillColor.Purple: //攻撃の反射
                user.AddEffect(EffectType.Reflect, 99);
                logMessage = $"{user.characterName} Barrier";
                break;
            case SkillColor.Special:
                target.TakeDamage(target.maxHp);
                logMessage = $"{user.characterName}'s special move! Dealt {target.maxHp} damage to {target.characterName}.";
                break;
        }

        return damage;
    }
}
