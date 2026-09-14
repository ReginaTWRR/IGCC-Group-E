using UnityEngine;
using UnityEngine.InputSystem;

// NPC script for conversations　会話で使用するNPCのスクリプト
public class NPCController : MonoBehaviour
{
    [Header("Dialogue Data 「会話データ」")]

    // Conversation JSON (English)　会話Json(英語)
    public TextAsset dialogueJsonEnglish;

    // Conversation JSON (Japanese)　会話Json(日本語)
    public TextAsset dialogueJsonJapanese;

    // Check for nearby players　近くのプレイヤーがいるか調べる
    private bool isPlayerNearby = false;


    void Update()
    {
        // Whether there is a player nearby and the Q key is being pressed　近くにプレイヤーがいるのとQキーを押しているか
        if ((isPlayerNearby) && (Keyboard.current != null) && (Keyboard.current.qKey.wasPressedThisFrame))
        {
            if ((!DialogueManager.Instance.IsDialogueActive) && (!DialogueManager.Instance.IsClosedThisFrame))
            {
                // Start a conversation 会話を開始する
                if (DialogueManager.Instance.IsWordedEnglish) DialogueManager.Instance.StartDialogue(dialogueJsonEnglish);
                else DialogueManager.Instance.StartDialogue(dialogueJsonJapanese);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = false;
    }
}
