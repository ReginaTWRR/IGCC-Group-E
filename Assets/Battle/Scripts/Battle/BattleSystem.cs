using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;


public class BattleSystem : MonoBehaviour
{
    [SerializeField] private BattleUIManager uiManager;

    [Header("Dataes")]
    [SerializeField] private CharacterData playerData;

    [SerializeField] private CharacterData enemyData;

    [Header("Inventory")]
    [SerializeField] private List<BattleItemData> inventory = new List<BattleItemData>();

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
            inventory.Add(new BattleItemData { itemName = "Potion", healAmount = 50, count = 2 });
        }

        uiManager.SetupUI(playerData, enemyData);
        uiManager.UpdateLog($"{enemyData.characterName} が あらわれた!");

        bool isPressed = false;
        using (var subscription = InputSystem.onAnyButtonPress.Call(control => isPressed = true))
        {
            yield return new WaitUntil(() => isPressed);
        }

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
                playerData.ChargeSpecialGauge((int)(poisonDamage*0.75));
                uiManager.UpdatePlayerUI(playerData);
                if (isPlayer) uiManager.UpdatePlayerUI(character);
                else uiManager.UpdateEnemyUI(character);

                uiManager.UpdateLog($"{character.characterName} は 毒によるダメージを受けた! (-{poisonDamage})");
                yield return new WaitForSeconds(3.0f);
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

        uiManager.UpdateLog("なにする?");
        uiManager.UpdatePlayerUI(playerData);
        uiManager.OpenActionPanel();
        yield return StartCoroutine(uiManager.UIAnimation(1));
    }


    public void OnMainAttackButton()
    {
        

        
        if (_currentState != BattleState.PlayerTurn) return;
        if (playerData.currentSpecialGauge < playerData.maxSpecialGauge)
        {
            StartCoroutine(MainAttackRoutine());
            IEnumerator MainAttackRoutine()
            {
                yield return StartCoroutine(uiManager.UIAnimation(0));

                List<AttackSkill> pool = new List<AttackSkill>(allSkills);
                for (int i = 0; i < 3; i++)
                {
                    if (pool.Count == 0) break;
                    int randomIndex = Random.Range(0, pool.Count-1);
                    _currentAttackOptions[i] = pool[randomIndex];
                    pool.RemoveAt(randomIndex);
                }

                uiManager.OpenAttackChoicePanel(_currentAttackOptions);
                yield return StartCoroutine(uiManager.UIAnimation(3));
            }
        }
        else
        {
            StartCoroutine(MainAttackRoutine());
            IEnumerator MainAttackRoutine()
            {
                yield return StartCoroutine(uiManager.UIAnimation(0));

                List<AttackSkill> pool = new List<AttackSkill>(allSkills);
                for (int i = 0; i < 3; i++)
                {
                    if (pool.Count == 0) break;
                    int randomIndex = pool.Count-1;
                    _currentAttackOptions[i] = pool[randomIndex];
                    
                }

                uiManager.OpenAttackChoicePanel(_currentAttackOptions,true);
                yield return StartCoroutine(uiManager.UIAnimation(4));
            }
        }
    }


    public void OnSelectAttackOption(int index)
    {
        if (_currentState != BattleState.PlayerTurn) return;
        StartCoroutine(ExecutePlayerAttack(_currentAttackOptions[index]));
    }

    private IEnumerator ExecutePlayerAttack(AttackSkill skill)
    {
        uiManager.HideAllPanels();

        if (skill.skillColor == SkillColor.Special && !playerData.IsSpecialReady)
        {

            playerData.currentSpecialGauge = 0;
        }


        int damage = skill.Execute(playerData, enemyData, out string log);
        uiManager.UpdateLog(log);
        uiManager.UpdateEnemyUI(enemyData);

        playerData.ChargeSpecialGauge((int)(damage*0.5));
        uiManager.UpdatePlayerUI(playerData);

        yield return new WaitForSeconds(3.0f);

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
                BattleItemData item = inventory[i];
                item.count--;
                inventory[i] = item;

                playerData.currentHp = Mathf.Clamp(playerData.currentHp + item.healAmount, 0, playerData.maxHp);
                uiManager.UpdatePlayerUI(playerData);
                uiManager.UpdateLog($"{item.itemName} を　つかった ! {item.healAmount} 回復した （のこり {item.count} 個）");
                itemUsed = true;
                break;
            }
        }

        if (!itemUsed)
        {
            uiManager.UpdateLog("使えない!");
            yield return new WaitForSeconds(3.0f);
            uiManager.OpenActionPanel();
            yield break;
        }

        yield return new WaitForSeconds(3.0f);
        ChangeState(BattleState.EnemyTurn);
    }

    public void OnMainPassButton()
    {
        if (_currentState != BattleState.PlayerTurn) return;
        uiManager.HideAllPanels();
        uiManager.UpdateLog("敵を観察している");
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
        yield return new WaitForSeconds(3.0f);
        uiManager.UpdateEnemyUI(enemyData);


        AttackSkill enemySkill = allSkills[Random.Range(0, allSkills.Count)];


        int previousHp = playerData.currentHp;

        int damage = enemySkill.Execute(enemyData, playerData, out string log);
        uiManager.UpdateLog(log);
        uiManager.UpdatePlayerUI(playerData);


        if (playerData.currentHp < previousHp)
        {
            playerData.ChargeSpecialGauge((int)(damage*0.75));
            uiManager.UpdatePlayerUI(playerData);
        }

        uiManager.UpdateEnemyUI(enemyData);

        yield return new WaitForSeconds(3f);
        
        if (playerData.IsDead) ChangeState(BattleState.Lost);
        else ChangeState(BattleState.PlayerTurn);
    }

    private void EndBattle(bool isWon)
    {
        uiManager.HideAllPanels();
        uiManager.UpdateLog(isWon ? $"{enemyData.characterName} を倒した!" : "負け");
    }


}
