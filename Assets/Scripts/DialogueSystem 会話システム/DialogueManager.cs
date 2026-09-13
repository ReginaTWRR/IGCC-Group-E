using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[System.Serializable]

// DialogueData　会話データ
public class DialogueData
{
    // Name　名前
    public string name;

    // Icon　アイコン
    public string icon;

    // Conversation text　会話テキスト
    public string text;
}


[System.Serializable]

// DialogueList　会話リスト
public class DialogueList
{
    // List conversation data　会話データをリスト化
    public List<DialogueData> dialogueList;
}


// Dialogue Manager　ダイアログマネージャー
public class DialogueManager : PersistentSingleton<DialogueManager>
{
    [Header("UI Elements 「UI」")]

    // Dialog box　ダイアログボックス
    public GameObject dialogueBox;

    // Name Text Mesh　名前テキストメッシュ
    public TextMeshProUGUI nameText;

    // Content Text Mesh　コンテンツテキストメッシュ
    public TextMeshProUGUI contentText;

    // NPC icon image　NPCアイコンイメージ
    public Image npcIconImage;


    [Header("System 「設定」")]

    // Speed ​​at which a single character appears　1文字出る速度
    public float typeSpeed = 0.05f;

    // Dialog scaling duration　ダイアログの拡大縮小する時間
    public float scaleDuration = 0.2f;

    // Current dialog　現在のダイアログ
    private List<DialogueData> currentDialogues = new List<DialogueData>();

    // Current index　現在のインデックス
    private int currentIndex = 0;

    // Check if typing is complete　タイピングが完了したか調べる
    private bool isTyping = false;

    // Check if the dialog is active　ダイアログがアクティブになっているか調べる
    private bool isDialogueActive = false;

    // Check if the frame has opened　フレームが開いたか調べる
    private bool openedThisFrame = false;

    // Coroutine typing　コルーチンの型指定
    private Coroutine typingCoroutine;

    // Function to start a conversation　会話を開始する関数
    public void StartDialogue(TextAsset jsonFile)
    {
        if (isDialogueActive) return;

        // Load JSON　Jsonを読み込む
        DialogueList data = JsonUtility.FromJson<DialogueList>(jsonFile.text);
        if ((data == null) || (data.dialogueList == null) || (data.dialogueList.Count == 0)) return;

        // Configure the settings　各設定を行う
        currentDialogues = data.dialogueList;
        currentIndex = 0;
        isDialogueActive = true;
        openedThisFrame = true;

        dialogueBox.SetActive(true);
        StartCoroutine(ScaleUI(Vector3.one));

        // Automatically start displaying the first line of dialogue　最初のセリフを自動で表示開始する
        DisplayNextSentence();
    }

    void Update()
    {
        if (!isDialogueActive) return;

        if (openedThisFrame)
        {
            openedThisFrame = false;
            return;
        }

        // Processing when the C key is pressed during a conversation　会話中にCキーを押した時の処理
        if ((Keyboard.current != null) && (Keyboard.current.cKey.wasPressedThisFrame))
        {
            if (isTyping)
            {
                // If pressed while typing, it instantly displays the full text　タイピング中に押されたら、一瞬で全文を表示する
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                contentText.text = currentDialogues[currentIndex].text;
                isTyping = false;
            }
            else
            {
                // If pressed while the full text is being displayed, proceed to the next line of dialogue　全文表示中に押されたら、次のセリフへ
                currentIndex++;
                DisplayNextSentence();
            }
        }
    }

    // A function that displays the next text　次の文章を表示する関数
    private void DisplayNextSentence()
    {
        // End the conversation after all lines have been spoken　全てのセリフが終わったら会話を終了
        if (currentIndex >= currentDialogues.Count)
        {
            EndDialogue();
            return;
        }

        // Start typing the next line　次のセリフのタイピングを開始
        if (typingCoroutine != null) StopCoroutine(TypeSentence(currentDialogues[currentIndex]));
        typingCoroutine = StartCoroutine(TypeSentence(currentDialogues[currentIndex]));
    }

    // Function for entering text　文を入力する関数
    private IEnumerator TypeSentence(DialogueData dialogue)
    {
        // Configure the settings　各設定を行う
        isTyping = true;
        nameText.text = dialogue.name;
        contentText.text = "";

        if (npcIconImage != null)
        {
            if (!string.IsNullOrEmpty(dialogue.icon))
            {
                // Load a sprite from the specified icon　指定したアイコンからスプライトを読み込む
                Sprite iconSprite = Resources.Load<Sprite>("Texture/NPC/" + dialogue.icon);
                if (iconSprite != null)
                {
                    npcIconImage.gameObject.SetActive(true);
                    npcIconImage.sprite = iconSprite;
                }
                else npcIconImage.gameObject.SetActive(true);
            }
            else npcIconImage.gameObject.SetActive(false);
        }
        foreach (char letter in dialogue.text.ToCharArray())
        {
            contentText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }

    // Function to close the dialog　ダイアログを終了する関数
    private void EndDialogue()
    {
        isDialogueActive = false;

        // Hide the UI while scaling it down　UIを縮小しながら非表示にする
        StartCoroutine(ScaleUI(Vector3.zero, () => dialogueBox.SetActive(false)));
    }

    // UI shrinking animation　UIの縮小アニメーション
    private IEnumerator ScaleUI(Vector3 targetScale, System.Action onComplete = null)
    {
        float time = 0;
        Vector3 startScale = dialogueBox.transform.localScale;

        while (time < scaleDuration)
        {
            dialogueBox.transform.localScale = Vector3.Lerp(startScale, targetScale, time / scaleDuration);
            time += Time.deltaTime;
            yield return null;
        }

        dialogueBox.transform.localScale = targetScale;
        onComplete?.Invoke();
    }

    // A property for checking from the outside whether a conversation is in progress　外部から会話中かどうかを確認するためのプロパティ
    public bool IsDialogueActive => isDialogueActive;
}
