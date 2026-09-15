using UnityEngine;
using UnityEngine.InputSystem;

// Script for switching players　プレイヤーの切り替えに関するスクリプト
public class PlayerChangeController : MonoBehaviour
{
    [Header("Object 「オブジェクト」")]

    // Coffin Object　棺桶オブジェクト
    [SerializeField] private GameObject coffinObject;


    [Header("Camera 「カメラ」")]

    // POV camera　視点カメラ
    [SerializeField] private Camera mainCamera;


    [Header("Input 「入力」")]

    // Input Device　入力デバイス
    [SerializeField] private PlayerInput inputDevice;


    [Header("System 「設定」")]

    // CharacterController　キャラクターコントローラー
    [SerializeField] private CharacterController characterController;

    // Directly store the reference to the generated coffin　生成した棺桶の参照を直接保存
    private GameObject spawnedCoffin;

    void Start()
    {
        // Set input device to Human　入力デバイスを人間に設定
        inputDevice.SwitchCurrentActionMap("Human");
    }

    void Update()
    {
        if ((Keyboard.current != null) && (Keyboard.current.tabKey.wasPressedThisFrame))
        {
            ChangeLanguage();
        }
    }

    //Function to change to a ghost　プレイヤーを幽霊に変更する関数
    void OnChangeGhost(InputValue value)
    {
        if ((!value.isPressed) || (DialogueManager.Instance.IsDialogueActive)) return;

        if (spawnedCoffin != null) return;

        // Set input device to Ghost　入力デバイスを幽霊に設定
        inputDevice.SwitchCurrentActionMap("Ghost");
        mainCamera.GetComponent<CameraPersonController>().TogglePerspective();

        // The position of the coffin　棺桶の位置
        Vector3 coffinPosition = transform.position;
        coffinPosition.y = transform.position.y - 0.5f;

        // Spawn a coffin at the player's position　棺桶をプレイヤーの位置に生成する
        spawnedCoffin = Instantiate(coffinObject, coffinPosition, transform.rotation);

        //When the character becomes a ghost, disable the CharacterController so it can pass through walls.　ゴースト化の時は CharacterController を無効にして壁をすり抜けられるようにする
        if (characterController != null) characterController.enabled = false;

        // Adjusted the settings to slightly raise the player's Y-axis position　プレイヤーのY軸位置を少し上げるように設定
        transform.position = new Vector3(transform.position.x, coffinPosition.y + 5.0f, transform.position.z);

        // Ask tbe TransformationMonitor to react
        // 変換モニターに反応するように要求する
        TransformationMonitor.Instance.React(true);
    }

    // Function to change to a human プレイヤーを人間に変更する関数
    void OnChangeHuman(InputValue value)
    {
        if ((!value.isPressed) || (DialogueManager.Instance.IsDialogueActive)) return;

        if (spawnedCoffin != null)
        {
            // Set input device to Human　入力デバイスを人間に設定
            inputDevice.SwitchCurrentActionMap("Human");

            // Switch perspectives　視点を切り替える
            mainCamera.GetComponent<CameraPersonController>().TogglePerspective();

            //Align the coffin with the player's position 棺桶の位置をプレイヤーの位置に合わせる
            transform.position = spawnedCoffin.transform.position;

            //Enable CharacterController when reverting to human form 人間状態に戻す時は CharacterController を有効にする
            if (characterController != null) characterController.enabled = true;

            // Delete the generated coffin　生成した棺桶を削除する
            GameObject.Destroy(spawnedCoffin);
            spawnedCoffin = null;

            // Ask tbe TransformationMonitor to react
            // 変換モニターに反応するように要求する
            TransformationMonitor.Instance.React(false);
        }
    }

    // Function to switch languages　言語を切り替える関数
    void ChangeLanguage()
    {
        if (DialogueManager.Instance.IsDialogueActive) return;

        if (DialogueManager.Instance.IsWordedEnglish) DialogueManager.Instance.SetWordlanguage(false);
        else DialogueManager.Instance.SetWordlanguage(true);
    }
}

