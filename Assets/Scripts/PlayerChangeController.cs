using UnityEngine;
using UnityEngine.InputSystem;

// Script for switching players　プレイヤーの切り替えに関するスクリプト
public class PlayerChangeController : MonoBehaviour
{
    [Header("Object 「オブジェクト」")]

    // Human Object　人間オブジェクト
    [SerializeField] private GameObject humanObject;

    // Ghost Object　幽霊オブジェクト
    [SerializeField] private GameObject ghostObject;

    // Grave Object　墓オブジェクト
    [SerializeField] private GameObject graveObject;


    [Header("Camera 「カメラ」")]

    // POV camera　視点カメラ
    [SerializeField] private Camera mainCamera;


    [Header("Input 「入力」")]

    // Input Device　入力デバイス
    [SerializeField] private PlayerInput inputDevice;


    //Function to change to a ghost　プレイヤーを幽霊に変更する関数
    void OnChangeGhost()
    {
        // Set object active state　オブジェクトのアクティブを設定
        humanObject.SetActive(false);
        ghostObject.SetActive(true);

        // Set input device to Ghost　入力デバイスを幽霊に設定
        inputDevice.SwitchCurrentActionMap("Ghost");

        // Switch perspectives　視点を切り替える
        mainCamera.GetComponent<CameraPersonController>().TogglePerspective();

        // Spawn a grave at the player's position　墓をプレイヤーの位置に生成する
        Instantiate(graveObject, transform.position, transform.rotation);
    }

    //Function to change to a human　プレイヤーを人間に変更する関数
    void OnChangeHuman()
    {
        // Set object active state　オブジェクトのアクティブを設定
        humanObject.SetActive(true);
        ghostObject.SetActive(false);

        // Set input device to Human　入力デバイスを人間に設定
        inputDevice.SwitchCurrentActionMap("Human");

        // Switch perspectives　視点を切り替える
        mainCamera.GetComponent<CameraPersonController>().TogglePerspective();

        // Delete the generated grave　生成した墓を削除する
        GameObject deleteObject = GameObject.Find("Grave(Clone)");
        if (deleteObject != null) GameObject.Destroy(deleteObject);
    }
}
