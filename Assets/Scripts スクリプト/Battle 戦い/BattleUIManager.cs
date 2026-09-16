
using System.Collections;
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
    [SerializeField] private GameObject attackChoicePanel;

    [Header("Attack Sub Buttons")]
    [SerializeField] private Button[] attackButtons;

    [Header("Effect Icons")]
    [SerializeField] private GameObject[] PlayerEffect;
    [SerializeField] private GameObject[] EnemyEffect;

    [Header("Shield")]

    [SerializeField] private GameObject PlayerShield;
    [SerializeField] private GameObject EnemyShield;


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
        playerSpecialSlider.maxValue = player.maxSpecialGauge;
        playerSpecialSlider.value = player.currentSpecialGauge;
        UpdatePlayerEffectIcon(player);
    }

    public void UpdateEnemyUI(CharacterData enemy)
    {
        enemyHpSlider.maxValue = enemy.maxHp;
        enemyHpSlider.value = enemy.currentHp;
        UpdateEnemyEffectIcon(enemy);
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

    public void OpenAttackPanel()
    {
        attackChoicePanel.SetActive(true);
    }


    public void OpenAttackChoicePanel(AttackSkill[] options)
    {
        
        HideAllPanels();
        OpenAttackPanel();
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

    public void OpenAttackChoicePanel(AttackSkill[] options,bool isSpecial)
    {

        HideAllPanels();
        OpenAttackPanel();
        for (int i = 0; i < attackButtons.Length; i++)
        {
            if (attackButtons[i] == null) continue;
            if(i!=1)
            {
                attackButtons[i].gameObject.SetActive(false);
            }

            

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

    public void UpdatePlayerEffectIcon(CharacterData player)
    {
        PlayerEffect[0].SetActive(player.HasDebuff());
        PlayerEffect[1].SetActive(player.HasPoison());
        PlayerEffect[2].SetActive(player.HasReflect());
        PlayerEffect[3].SetActive(player.HasBlind());
        PlayerShield.SetActive(player.HasShield());

        if (player.HasDebuff())
        {
            Vector3 currentScale = PlayerEffect[0].transform.localScale;
            currentScale.y = (float)(player.HasDebuffTurn()) / 3.0f;
            PlayerEffect[0].transform.localScale = currentScale;
        }
        if (player.HasPoison())
        {
            Vector3 currentScale = PlayerEffect[1].transform.localScale;
            currentScale.y = (float)(player.HasPoisonTurn()) / 3.0f;
            PlayerEffect[1].transform.localScale = currentScale;
        }
        if (player.HasBlind())
        {
            Vector3 currentScale = PlayerEffect[3].transform.localScale;
            currentScale.y = (float)(player.HasBlindTurn()) / 3.0f;
            PlayerEffect[3].transform.localScale = currentScale;
        }
    }

    public void UpdateEnemyEffectIcon(CharacterData enemy)
    {
        EnemyEffect[0].SetActive(enemy.HasDebuff());
        EnemyEffect[1].SetActive(enemy.HasPoison());
        EnemyEffect[2].SetActive(enemy.HasReflect());
        EnemyEffect[3].SetActive(enemy.HasBlind());
        EnemyShield.SetActive(enemy.HasShield());

        if (enemy.HasDebuff())
        {
            Vector3 currentScale = EnemyEffect[0].transform.localScale;
            currentScale.y = (float)(enemy.HasDebuffTurn()) / 3.0f;
            EnemyEffect[0].transform.localScale = currentScale;
        }
        if (enemy.HasPoison())
        {
            Vector3 currentScale = EnemyEffect[1].transform.localScale;
            currentScale.y = (float)(enemy.HasPoisonTurn()) / 3.0f;
            EnemyEffect[1].transform.localScale = currentScale;
        }
        if (enemy.HasBlind())
        {
            Vector3 currentScale = EnemyEffect[3].transform.localScale;
            currentScale.y = (float)(enemy.HasBlindTurn()) / 3.0f;
            EnemyEffect[3].transform.localScale = currentScale;
        }
    }


    public IEnumerator UIAnimation(int index)
    {
        switch(index)
        {
            case 0:
                {
                    float progress = 100.0f;

                    while (progress > 0.0f)
                    {
                        Vector3 currentScale = actionChoicePanel.transform.localScale;
                        currentScale.x = progress / 100.0f;
                        actionChoicePanel.transform.localScale = currentScale;

                        progress -= Time.deltaTime * 100f;

                        yield return null;
                    }

                    Vector3 finalScale = actionChoicePanel.transform.localScale;
                    finalScale.x = 0f;
                    actionChoicePanel.transform.localScale = finalScale;
                    actionChoicePanel.SetActive(false);
                }
                break;
            case 1:
                {
                    float progress = 0.0f;

                    while (progress < 100.0f)
                    {
                        Vector3 currentScale = actionChoicePanel.transform.localScale;
                        currentScale.x = progress / 100.0f;
                        actionChoicePanel.transform.localScale = currentScale;

                        progress += Time.deltaTime * 100f;

                        yield return null;
                    }

                    Vector3 finalScale = actionChoicePanel.transform.localScale;
                    finalScale.x = 1.0f;
                    actionChoicePanel.transform.localScale = finalScale;

                }
                break;
            case 2:
                {
                    float progress = 100.0f;

                    while (progress > 0.0f)
                    {
                        Vector3 currentScale = attackChoicePanel.transform.localScale;
                        currentScale.x = progress / 100.0f;
                        attackChoicePanel.transform.localScale = currentScale;

                        progress -= Time.deltaTime * 100f;

                        yield return null;
                    }

                    Vector3 finalScale = attackChoicePanel.transform.localScale;
                    finalScale.x = 0f;
                    attackChoicePanel.transform.localScale = finalScale;
                    attackChoicePanel.SetActive(false);
                }
                break;
            case 3:
                {


                    float progress = 0.0f;
                    var buttonImage1 = attackButtons[0].GetComponent<Image>();
                    var buttonImage2 = attackButtons[1].GetComponent<Image>();
                    var buttonImage3 = attackButtons[2].GetComponent<Image>();
                    Color32 CurrentColor1 = buttonImage1.color;
                    buttonImage1.color = new Color32(CurrentColor1.r, CurrentColor1.g, CurrentColor1.b, (byte)(255.0f * progress / 100.0f));
                    Color32 CurrentColor2 = buttonImage2.color;
                    buttonImage2.color = new Color32(CurrentColor2.r, CurrentColor2.g, CurrentColor2.b, (byte)(255.0f * progress / 100.0f));
                    Color32 CurrentColor3 = buttonImage3.color;
                    buttonImage3.color = new Color32(CurrentColor3.r, CurrentColor3.g, CurrentColor3.b, (byte)(255.0f * progress / 100.0f));

                    
                    while(progress<160.0f)
                    {
                        
                        if(0.0f <= progress && progress <= 100.0f)
                        {
                            buttonImage1.color = new Color32(CurrentColor1.r, CurrentColor1.g, CurrentColor1.b, (byte)(255.0f * progress / 100.0f));
                        }
                        if (30.0f <= progress && progress <= 130.0f)
                        {
                            buttonImage2.color = new Color32(CurrentColor2.r, CurrentColor2.g, CurrentColor2.b, (byte)(255.0f * (progress - 30.0f) / 100.0f));
                        }
                        if (60.0f <= progress && progress <= 160.0f)
                        {
                            buttonImage3.color = new Color32(CurrentColor3.r, CurrentColor3.g, CurrentColor3.b, (byte)(255.0f * (progress - 60.0f) / 100.0f));
                        }
                        progress += Time.deltaTime * 100f;

                        yield return null;
                    }
                }
                break;
            case 4:
                {


                    float progress = 0.0f;
                    var buttonImage1 = attackButtons[0].GetComponent<Image>();
                    var buttonImage2 = attackButtons[1].GetComponent<Image>();
                    var buttonImage3 = attackButtons[2].GetComponent<Image>();
                    Color32 CurrentColor1 = buttonImage1.color;
                    buttonImage1.color = new Color32(CurrentColor1.r, CurrentColor1.g, CurrentColor1.b, (byte)(255.0f * progress / 100.0f));
                    Color32 CurrentColor2 = buttonImage2.color;
                    buttonImage2.color = new Color32(CurrentColor2.r, CurrentColor2.g, CurrentColor2.b, (byte)(255.0f * progress / 100.0f));
                    Color32 CurrentColor3 = buttonImage3.color;
                    buttonImage3.color = new Color32(CurrentColor3.r, CurrentColor3.g, CurrentColor3.b, (byte)(255.0f * progress / 100.0f));


                    while (progress < 160.0f)
                    {

                        if (0.0f <= progress && progress <= 100.0f)
                        {
                            buttonImage1.color = new Color32(CurrentColor1.r, CurrentColor1.g, CurrentColor1.b, (byte)(255.0f * progress / 100.0f));
                           
                        }
                        if (30.0f <= progress && progress <= 130.0f)
                        {
                            buttonImage2.color = new Color32(CurrentColor2.r, CurrentColor2.g, CurrentColor2.b, (byte)(255.0f * (progress - 30.0f) / 100.0f));
                        }
                        if (60.0f <= progress && progress <= 160.0f)
                        {
                            buttonImage3.color = new Color32(CurrentColor3.r, CurrentColor3.g, CurrentColor3.b, (byte)(255.0f * (progress - 60.0f) / 100.0f));
                        }
                        progress += Time.deltaTime * 100f;

                        yield return null;
                    }
                }
                break;

            default: break;
        }

        
    }
}
