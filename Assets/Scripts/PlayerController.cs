using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("System 設定")]
    // Human Object　人間オブジェクト
    public GameObject humanObject;

    // Ghost Object　幽霊オブジェクト
    public GameObject ghostObject;

    // Camera　カメラ
    public Camera playerCamera;

    // Input Device　入力デバイス
    public PlayerInput inputDevice;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //Function to change to a ghost　プレイヤーを幽霊に変更する関数
    void OnChangeGhost()
    {
        humanObject.SetActive(false);
        ghostObject.SetActive(true);

        // Set input device to Ghost　入力デバイスを幽霊に設定
        inputDevice.SwitchCurrentActionMap("Ghost");
    }

    //Function to change to a human　プレイヤーを人間に変更する関数
    void OnChangeHuman()
    {
        humanObject.SetActive(true);
        ghostObject.SetActive(false);

        // Set input device to Human　入力デバイスを人間に設定
        inputDevice.SwitchCurrentActionMap("Human");
    }
}
