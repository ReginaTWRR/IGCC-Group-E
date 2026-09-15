using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

// NPC script for conversations　会話で使用するNPCのスクリプト
public class NPCController : MonoBehaviour
{
    [Header("Object 「オブジェクト」")]

    // Player Object　プレイヤーオブジェクト
    [SerializeField] private GameObject playerObject;


    [Header("Dialogue Data 「会話データ」")]

    // Conversation JSON (English)　会話Json(英語)
    public TextAsset dialogueJsonEnglish;

    // Conversation JSON (Japanese)　会話Json(日本語)
    public TextAsset dialogueJsonJapanese;

    // Check for nearby players　近くのプレイヤーがいるか調べる
    private bool isPlayerNearby = false;


    [Header("System 「設定」")]

    // Interpolation speed　補間速度
    [SerializeField] private float transitionSpeed = 2.0f;

    // Initial rotational position　初期回転位置
    private Quaternion initialRotatetion;

    void Start()
    {
        // Save initial rotational position　初期回転位置を保存
        initialRotatetion = transform.rotation;
    }

    void Update()
    {
        // Are there any players nearby　近くにプレイヤーがいるか
        if (isPlayerNearby)
        {
            // direction　方向
            Vector3 direction = (playerObject.transform.position - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                quaternion rotate = Quaternion.LookRotation(direction);

                // Interpolated movement toward the target　ターゲットに向かって補間移動
                transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * transitionSpeed);
            }

            // Are you pressing the Q key　Qキーを押しているか
            if ((Keyboard.current != null) && (Keyboard.current.qKey.wasPressedThisFrame))
            {
                if ((!DialogueManager.Instance.IsDialogueActive) && (!DialogueManager.Instance.IsClosedThisFrame))
                {
                    // Start a conversation 会話を開始する
                    if (DialogueManager.Instance.IsWordedEnglish) DialogueManager.Instance.StartDialogue(dialogueJsonEnglish);
                    else DialogueManager.Instance.StartDialogue(dialogueJsonJapanese);
                }
            }
        } 
        else
        {
            // Interpolated movement toward the target　ターゲットに向かって補間移動
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotatetion, Time.deltaTime * transitionSpeed);
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
