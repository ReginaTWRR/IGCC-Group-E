using UnityEngine;

public class DoorManager : PersistentSingleton<DoorManager>
{
    LockedDoor nearestDoor;
    
    public void SetNearestDoor(LockedDoor nearestDoor)
    {
        this.nearestDoor = nearestDoor;
    }

    public void TryUnlockDoor()
    {
        if (nearestDoor == null) return;

        // Turn all the colliders off
        // すべてのコライダーをオフにする
        foreach (Collider cld in nearestDoor.colliders)
        {
            cld.enabled = false;
        }
    }
}
