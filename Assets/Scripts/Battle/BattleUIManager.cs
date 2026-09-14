using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    [Header("Player UI")]
    [SerializeField] private Slider playerHpSlider;
    [SerializeField] private Slider playerSpecialSlider;

    [Header("Enemy UI")]
    [SerializeField] private Slider enemyHpSlider;
    [SerializeField] private TextMeshProUGUI enemyNameText;

    [Header("Log UI")]
    [SerializeField] private TextMeshProUGUI logText;

    [Header("Panels")]
    [SerializeField] private GameObject actionChoicePanel;
    [SerializeField] private GameObject attackChoicePanel; // 赤いパネル（AttackPanel）

    // 【最適化変更】テキストではなく、ボタン本体を直接登録する形に変更
    [Header("Attack Sub Buttons")]
    [SerializeField] private Button[] attackButtons;

    public void SetupUI(CharacterData player, CharacterData enemy)
    {
        enemyNameText.text = enemy.characterName;
        UpdatePlayerUI(player);
        UpdateEnemyUI(enemy);
        HideAllPanels();
    }



    public void UpdatePlayerUI(CharacterData player)
    {
        playerHpSlider.maxValue = player.maxHp;
        playerHpSlider.value = player.currentHp;
        playerSpecialSlider.value = player.currentSpecialGauge;
    }

    public void UpdateEnemyUI(CharacterData enemy)
    {
        enemyHpSlider.maxValue = enemy.maxHp;
        enemyHpSlider.value = enemy.currentHp;
    }

    public void UpdateLog(string message)
    {
        logText.text = message;
    }

    public void OpenActionPanel()
    {
        HideAllPanels();
        actionChoicePanel.SetActive(true);
    }


    public void OpenAttackChoicePanel(AttackSkill[] options)
    {
        HideAllPanels();
        attackChoicePanel.SetActive(true);
        for (int i = 0; i < attackButtons.Length; i++)
        {
            if (attackButtons[i] == null) continue;


            attackButtons[i].gameObject.SetActive(true);

            var buttonImage = attackButtons[i].GetComponent<Image>();

            if (buttonImage != null)
            {
                buttonImage.color = new Color32(options[i].Color_R, options[i].Color_G, options[i].Color_B, 255);
            }

            // 子オブジェクトにあるTMPテキストを取得して文字を書き換える
            TextMeshProUGUI btnText = attackButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            if (i < options.Length && options[i] != null)
            {
                if (btnText != null) btnText.text = options[i].skillName;
                attackButtons[i].interactable = true; // 押せるようにする
            }
            else
            {
                if (btnText != null) btnText.text = "_____";
                attackButtons[i].interactable = false; // 選択肢がない枠は半透明にして押せなくする
            }
        }
    }

    public void HideAllPanels()
    {
        actionChoicePanel.SetActive(false);
        attackChoicePanel.SetActive(false);
    }
}
