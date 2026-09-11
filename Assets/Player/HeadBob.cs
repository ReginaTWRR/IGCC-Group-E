using UnityEngine;
using UnityEngine.InputSystem;

public class HeadBob : MonoBehaviour
{
    // Settings for head bobbing while walking (speed, amplitude, smoothness)
    //歩行時のヘッドボブの設定（揺れの速さ、揺れ幅、滑らかさ）
    [Header("Head Bob")]
    [SerializeField] float bobFrequency = 10f;
    [SerializeField] float bobAmplitude = 0.1f;
    [SerializeField] float bobSmooth = 10f;

    // Camera settings during jumps (vertical movement intensity, smoothness)
    //ジャンプ時のカメラ設定（上下移動の強さ、滑らかさ）
    [Header("Jump Camera")]
    [SerializeField] float jumpCameraHeight = 0.15f;
    [SerializeField] float jumpCameraSmooth = 8f;

    // Variable to store the camera's initial position
    // カメラの初期位置を保存する変数
    Vector3 initialPosition;

    // Timer for managing head-bob timing
    // ヘッドボブのタイミングを管理するタイマー
    float bobTimer;

    // Get the player's movement speed and ground contact status
    // プレイヤーの移動速度や接地状態を取得
    CharacterController characterController;

    void Start()
    {
        initialPosition = transform.localPosition;

        characterController = GetComponentInParent<CharacterController>();
    }

    void Update()
    {
        Vector3 velocity = characterController.velocity;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);


        // When in the air
        // 空中にいる場合
        if (!characterController.isGrounded)
        {
            // Move the camera using the ascent and descent speeds
            // 上昇・下降速度を使ってカメラを動かす
            float jumpOffset = Mathf.Clamp(velocity.y * jumpCameraHeight,-jumpCameraHeight,jumpCameraHeight);
            Vector3 target = initialPosition + new Vector3(0, jumpOffset, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition,target,Time.deltaTime * jumpCameraSmooth);

            // Reset the head bob for walking
            // 歩行用ヘッドボブをリセット
            bobTimer = 0;

            return;
        }

        // When on the ground
        // 地面にいる場合
        if (horizontalVelocity.magnitude > 0.1f)
        {
            // Update the timing of the head bob
            //ヘッドボブのタイミングを更新
            bobTimer += Time.deltaTime * bobFrequency;

            // Vertical oscillation using a sine wave
            // Sin波を使った上下方向の揺れ
            float bobY = Mathf.Sin(bobTimer) * bobAmplitude;

            // Left-right sway using a cosine wave
            // Cos波を使った左右方向の揺れ
            float bobX = Mathf.Cos(bobTimer * 0.5f) * bobAmplitude * 0.5f;

            // Do not move in the Z direction
            // Z方向には移動させない
            Vector3 target = initialPosition + new Vector3(bobX, bobY, 0);

            // Move the camera to the position it would be in while walking
            // カメラを歩行時の揺れ位置へ移動
            transform.localPosition = Vector3.Lerp( transform.localPosition, target, Time.deltaTime * bobSmooth );
        }
        else
        {
            // Reset the head-bob timer
            // ヘッドボブのタイマーをリセット
            bobTimer = 0;

            // Smoothly return the camera to its initial position
            // カメラを初期位置へ滑らかに戻す
            transform.localPosition = Vector3.Lerp(transform.localPosition,initialPosition,Time.deltaTime * bobSmooth);
        }
    }
}
