using UnityEngine;
using System.Collections.Generic;

public class Door : MonoBehaviour
{
    [Header("Door")]
    [SerializeField] List<Collider> colliders;

    private void OnTriggerEnter(Collider other)
    {
        DoorManager.Instance.SetNearestDoor(new(this, colliders));
    }

    private void OnTriggerExit(Collider other)
    {
        DoorManager.Instance.SetNearestDoor(null);
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the colliders
        // collidersを追加する
        colliders.Clear();
        Collider[] colliderComponents = GetComponentsInChildren<Collider>();
        colliders = new(colliderComponents);
    }
#endif
}
