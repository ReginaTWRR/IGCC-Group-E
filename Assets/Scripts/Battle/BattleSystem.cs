using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private BattleUIManager uiManager;

    [Header("Dataes")]
    //[SerializeField] private CharacterData playerData;
    [SerializeField] private CharacterData playerData;
    //[SerializeField] private CharacterData enemyData;
    [SerializeField] private CharacterData enemyData;

    [Header("Inventory")]
    [SerializeField] private List<ItemData> inventory = new List<ItemData>();

    [Header("All Available Skills")]
    [SerializeField] private List<AttackSkill> allSkills; // 5色のスキルプール

    private BattleState _currentState;
    private AttackSkill[] _currentAttackOptions = new AttackSkill[3]; // 選択肢キャッシュ

    private void Start()
    {
        _currentState = BattleState.Setup;
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        playerData.currentHp = playerData.maxHp;
        playerData.currentSpecialGauge = 0;
        enemyData.currentHp = enemyData.maxHp;

        // アイテム初期化
        if (inventory.Count == 0)
        {
            inventory.Add(new ItemData { itemName = "Potion", healAmount = 50, count = 3 });
        }

        uiManager.SetupUI(playerData, enemyData);
        uiManager.UpdateLog($"{enemyData.characterName} ga arawareta!");

        yield return new WaitForSeconds(1.0f);

        //先手はプレイヤーから
        ChangeState(BattleState.PlayerTurn);
    }

    private void ChangeState(BattleState newState)
    {
        _currentState = newState;
        switch (_currentState)
        {
            case BattleState.PlayerTurn: StartCoroutine(PlayerTurnRoutine()); break;
            case BattleState.EnemyTurn: StartCoroutine(EnemyTurnRoutine()); break;
            case BattleState.Won: EndBattle(true); break;
            case BattleState.Lost: EndBattle(false); break;
        }
    }

    //持続ダメージの処理
    private IEnumerator ProcessTurnEffects(CharacterData character, bool isPlayer)
    {
        for (int i = character.activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = character.activeEffects[i];

            if (effect.type == EffectType.Poison)
            {
                //最大HP参照の毒
                int poisonDamage = Mathf.Max(1, character.maxHp / 10);
                character.TakeDamage(poisonDamage);

                if (isPlayer) uiManager.UpdatePlayerUI(character);
                else uiManager.UpdateEnemyUI(character);

                uiManager.UpdateLog($"{character.characterName} ha dokunodame-ziwouketa! (-{poisonDamage})");
                yield return new WaitForSeconds(1.0f);
            }

            //ターン数の減算
            effect.durationTurns--;
            if (effect.durationTurns <= 0)
            {
                character.activeEffects.RemoveAt(i);
            }
            else
            {
                character.activeEffects[i] = effect;
            }
        }
    }

    private IEnumerator PlayerTurnRoutine()
    {
        yield return StartCoroutine(ProcessTurnEffects(playerData, true));
        if (playerData.IsDead)
        {
            ChangeState(BattleState.Lost);
            yield break;
        }

        uiManager.UpdateLog("dousuru?");
        uiManager.UpdatePlayerUI(playerData);
        uiManager.OpenActionPanel();
    }


    public void OnMainAttackButton()
    {
        if (_currentState != BattleState.PlayerTurn) return;

        List<AttackSkill> pool = new List<AttackSkill>(allSkills);
        for (int i = 0; i < 3; i++)
        {
            if (pool.Count == 0) break;
            int randomIndex = Random.Range(0, pool.Count);
            _currentAttackOptions[i] = pool[randomIndex];
            pool.RemoveAt(randomIndex);
        }

        uiManager.OpenAttackChoicePanel(_currentAttackOptions);
    }


    public void OnSelectAttackOption(int index)
    {
        if (_currentState != BattleState.PlayerTurn) return;
        StartCoroutine(ExecutePlayerAttack(_currentAttackOptions[index]));
    }

    private IEnumerator ExecutePlayerAttack(AttackSkill skill)
    {
        uiManager.HideAllPanels();

        if (skill.skillColor == SkillColor.Purple && !playerData.IsSpecialReady)
        {

            playerData.currentSpecialGauge = 0;
        }


        skill.Execute(playerData, enemyData, out string log);
        uiManager.UpdateLog(log);
        uiManager.UpdateEnemyUI(enemyData);

        playerData.ChargeSpecialGauge(50);
        uiManager.UpdatePlayerUI(playerData);

        yield return new WaitForSeconds(1.5f);

        if (enemyData.IsDead) ChangeState(BattleState.Won);
        else ChangeState(BattleState.EnemyTurn);
    }

    public void OnMainItemButton()
    {
        if (_currentState != BattleState.PlayerTurn) return;
        StartCoroutine(ExecutePlayerItem());
    }

    private IEnumerator ExecutePlayerItem()
    {
        uiManager.HideAllPanels();


        bool itemUsed = false;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].count > 0)
            {
                ItemData item = inventory[i];
                item.count--;
                inventory[i] = item;

                playerData.currentHp = Mathf.Clamp(playerData.currentHp + item.healAmount, 0, playerData.maxHp);
                uiManager.UpdatePlayerUI(playerData);
                uiManager.UpdateLog($"{item.itemName} wo use ! HP {item.healAmount} kaihukusita （nokori {item.count} cnt）");
                itemUsed = true;
                break;
            }
        }

        if (!itemUsed)
        {
            uiManager.UpdateLog("NOT USE!");
            yield return new WaitForSeconds(1.0f);
            uiManager.OpenActionPanel();
            yield break;
        }

        yield return new WaitForSeconds(1.5f);
        ChangeState(BattleState.EnemyTurn);
    }

    public void OnMainPassButton()
    {
        if (_currentState != BattleState.PlayerTurn) return;
        uiManager.HideAllPanels();
        uiManager.UpdateLog("Player see Enemy");
        Invoke(nameof(GoToEnemyTurn), 1.0f);
    }

    private void GoToEnemyTurn() => ChangeState(BattleState.EnemyTurn);

    private IEnumerator EnemyTurnRoutine()
    {
        yield return StartCoroutine(ProcessTurnEffects(enemyData, false));
        if (enemyData.IsDead)
        {
            ChangeState(BattleState.Won);
            yield break;
        }

        uiManager.UpdateLog($"{enemyData.characterName} 's ta-n");
        yield return new WaitForSeconds(1.0f);


        AttackSkill enemySkill = allSkills[Random.Range(0, allSkills.Count)];


        int previousHp = playerData.currentHp;

        enemySkill.Execute(enemyData, playerData, out string log);
        uiManager.UpdateLog(log);
        uiManager.UpdatePlayerUI(playerData);


        if (playerData.currentHp < previousHp)
        {
            playerData.ChargeSpecialGauge(75);
            uiManager.UpdatePlayerUI(playerData);
        }

        yield return new WaitForSeconds(1.5f);

        if (playerData.IsDead) ChangeState(BattleState.Lost);
        else ChangeState(BattleState.PlayerTurn);
    }

    private void EndBattle(bool isWon)
    {
        uiManager.HideAllPanels();
        uiManager.UpdateLog(isWon ? $"{enemyData.characterName} wo taosita!" : "Player lose ...");
    }
}
