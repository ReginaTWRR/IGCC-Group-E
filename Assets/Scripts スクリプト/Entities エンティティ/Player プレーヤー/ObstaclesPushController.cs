using UnityEngine;

// Script for the physics behavior of the coffin　棺桶の物理挙動に関するスクリプト
public class ObstaclesPushController : MonoBehaviour
{
    [Header("System 「設定」")]

    // Pushing force　押す力
    [SerializeField] private float pushPower = 2.0f;

    // The Rigidbody of the object that was contacted　接触したオブジェクトのRigidBody
    private Rigidbody targetRigidBody;

    // Check if it is being pressed　押しているか調べる
    private bool isPushing = false;

    void FixedUpdate()
    {
        if ((isPushing) && (targetRigidBody != null))
        {
            // Apply force in the direction the player is facing　プレイヤーの向いている方向に力に代入
            Vector3 pushDir = transform.forward;

            // Apply force　力を加える
            targetRigidBody.AddForce(pushDir * pushPower, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Get the Rigidbody of the collided object　衝突したオブジェクトのRigidBodyを取得する
        Rigidbody boxBody = collision.collider.attachedRigidbody;

        if ((boxBody != null) && (!boxBody.isKinematic))
        {
            isPushing = true;
            targetRigidBody = boxBody;

            targetRigidBody.WakeUp();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Get the Rigidbody of the collided object　衝突したオブジェクトのRigidBodyを取得する
        Rigidbody boxBody = collision.collider.attachedRigidbody;

        if (boxBody == targetRigidBody)
        {
            isPushing = false;
            targetRigidBody = null;
        }
    }
}
