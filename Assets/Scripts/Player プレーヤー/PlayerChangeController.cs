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

    // Coffin Object　棺桶オブジェクト
    [SerializeField] private GameObject coffinObject;


    [Header("Camera 「カメラ」")]

    // POV camera　視点カメラ
    [SerializeField] private Camera mainCamera;


    [Header("Input 「入力」")]

    // Input Device　入力デバイス
    [SerializeField] private PlayerInput inputDevice;


    [Header("Sytem 「設定」")]

    // Rigid Body
    [SerializeField] private Rigidbody rigidBody;

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

        // Spawn a coffin at the player's position　棺桶をプレイヤーの位置に生成する
        Instantiate(coffinObject, transform.position, transform.rotation);

        // Rigid Body Change　RigidBodyを変更
        rigidBody.isKinematic = true;
        rigidBody.useGravity = false;

        // Adjusted the settings to slightly raise the player's Y-axis position　プレイヤーのY軸位置を少し上げるように設定
        transform.position = new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z);
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

        // Find the generated coffin　生成した棺桶を探す
        GameObject deleteObject = GameObject.Find("Coffin(Clone)");
        if (deleteObject != null)
        {
            // 棺桶の位置をプレイヤーの位置に合わせる
            transform.position = deleteObject.transform.position;

            // Rigid Body Change　RigidBodyを変更
            rigidBody.isKinematic = false;
            rigidBody.useGravity = true;

            // Delete the generated coffin　生成した棺桶を削除する
            GameObject.Destroy(deleteObject);
        }
    }
}
