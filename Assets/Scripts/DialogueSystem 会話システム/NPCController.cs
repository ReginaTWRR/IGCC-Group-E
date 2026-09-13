using UnityEngine;
using UnityEngine.InputSystem;

// NPC script for conversations　会話で使用するNPCのスクリプト
public class NPCController : MonoBehaviour
{
    [Header("Dialogue Data 「会話データ」")]

    // Conversation JSON　会話Json
    public TextAsset dialogueJson;

    // Check for nearby players　近くのプレイヤーがいるか調べる
    private bool isPlayerNearby = false;


    void Update()
    {
        // Whether there is a player nearby and the C key is being pressed　近くにプレイヤーがいるのとCキーを押しているか
        if ((isPlayerNearby) && (Keyboard.current != null) && (Keyboard.current.cKey.wasPressedThisFrame))
        {
            if (!DialogueManager.Instance.IsDialogueActive)
            {
                // Start a conversation 会話を開始する
                DialogueManager.Instance.StartDialogue(dialogueJson);
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
