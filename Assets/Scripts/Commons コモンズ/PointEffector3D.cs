using UnityEngine;
using System.Collections.Generic;

public class PointEffector3D : MonoBehaviour
{
    [Header("Point Effector 3D")]
    [SerializeField] Collider clder;
    public LayerMask colliderMask;
    public float forceMagnitude;

    List<GameObject> objectsInRange = new();

    private void FixedUpdate()
    {
        // Make objects in range move towards the point
        // 範囲内のオブジェクトをその地点に向かって移動させる
        foreach (GameObject obj in objectsInRange)
        {
            // Ensure the object has a valid rigidbody
            // オブジェクトに有効なリジッドボディがあることを確認する
            if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb) == false) continue;
            if (rb.isKinematic) continue;

            // Calculate the direction from the object to the point
            // オブジェクトからその点までの方向を計算する
            Vector3 direction = transform.position - obj.transform.position;
            direction = direction.normalized;

            // Apply the force
            // 力を加える
            rb.AddForce(direction * forceMagnitude, ForceMode.Force);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (objectsInRange.Contains(other.gameObject) == false)
        {
            if (colliderMask.Contains(other.gameObject.layer))
            {
                objectsInRange.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveObject(other.gameObject);
    }

    public void RemoveObject(GameObject toRemove)
    {
        if (objectsInRange.Contains(toRemove))
        {
            objectsInRange.Remove(toRemove);
        }
    }
}
