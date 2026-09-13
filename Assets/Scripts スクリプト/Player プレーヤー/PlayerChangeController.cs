using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

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

    //Function to change to a ghost　プレイヤーを幽霊に変更する関数
    void OnChangeGhost(InputValue value)
    {
        if (!value.isPressed) return;

        if (spawnedCoffin != null) return;

        // Set input device to Ghost　入力デバイスを幽霊に設定
        inputDevice.SwitchCurrentActionMap("Ghost");

        // Switch perspectives　視点を切り替える
        mainCamera.GetComponent<CameraPersonController>().TogglePerspective();

        // The position of the coffin　棺桶の位置
        Vector3 coffinPosition = transform.position;
        coffinPosition.y = transform.position.y - 0.5f;

        // Spawn a coffin at the player's position　棺桶をプレイヤーの位置に生成する
        spawnedCoffin = Instantiate(coffinObject, coffinPosition, transform.rotation);

        if (characterController != null) characterController.enabled = false;

        // Adjusted the settings to slightly raise the player's Y-axis position　プレイヤーのY軸位置を少し上げるように設定
        transform.position = new Vector3(transform.position.x, coffinPosition.y + 5.0f, transform.position.z);

        if (characterController != null) characterController.enabled = true;
    }

    //Function to change to a human　プレイヤーを人間に変更する関数
    void OnChangeHuman(InputValue value)
    {
        if (!value.isPressed) return;

        if (spawnedCoffin != null)
        {
            // Set input device to Human　入力デバイスを人間に設定
            inputDevice.SwitchCurrentActionMap("Human");

            // Switch perspectives　視点を切り替える
            mainCamera.GetComponent<CameraPersonController>().TogglePerspective();

            if (characterController != null) characterController.enabled = false;

            // 棺桶の位置をプレイヤーの位置に合わせる
            transform.position = spawnedCoffin.transform.position;

            if (characterController != null) characterController.enabled = true;

            // Delete the generated coffin　生成した棺桶を削除する
            GameObject.Destroy(spawnedCoffin);
            spawnedCoffin = null;
        }
    }
}
