using UnityEngine;

// Script for changing the camera viewpoint　カメラの視点変更に関するスクリプト
public class CameraPersonController : MonoBehaviour
{
    [Header("Reference point for the viewpoint 「視点の基準点」")]

    // First person fixed position　1人称視点固定位置
    [SerializeField] private Transform firstPersonAnchor;

    // Third person fixed position　3人称視点固定位置
    [SerializeField] private Transform thistPersonAnchor;


    [Header("System 「設定」")]

    // Interpolation speed　補間速度
    [SerializeField] private float transitionSpeed = 5.0f;

    // Target fixation　標的固定
    private Transform targetAnchor;

    // Flag management for the first-person perspective　1人称視点かのフラグ管理
    private bool isFirstPerson = true;


    void Start()
    {
        // Set the initial viewpoint to first person　初期視点を1人称視点に設定
        targetAnchor = firstPersonAnchor;

        // Align the initial position and initial rotation　初期位置と初期回転を合わせる
        transform.position = targetAnchor.position;
        transform.rotation = targetAnchor.rotation;
    }

    void LateUpdate()
    {
        if (targetAnchor == null) return;

        // Interpolated movement toward the target　ターゲットに向かって補間移動
        transform.position = Vector3.Lerp(transform.position, targetAnchor.position, Time.deltaTime * transitionSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetAnchor.rotation, Time.deltaTime * transitionSpeed);
    }

    // Function to switch the viewpoint　視点を切り替える関数
    public void TogglePerspective()
    {
        // Toggle the flag　フラグを切り替える
        isFirstPerson = !isFirstPerson;

        // Switch targets　ターゲットを切り替える
        targetAnchor = isFirstPerson ? firstPersonAnchor : thistPersonAnchor;
    }
}
