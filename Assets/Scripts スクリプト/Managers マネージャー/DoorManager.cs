using UnityEngine;

public class DoorManager : PersistentSingleton<DoorManager>
{
    Door nearestDoor;

    public void SetNearestDoor(Door nearestDoor)
    {
        this.nearestDoor = nearestDoor;
    }

    public void TryUnlockDoor(KeyDoorMatchData match)
    {
        if (nearestDoor == null) return;
        if (nearestDoor.match.door != match.door) return;

        nearestDoor.UnlockDoor();
    }
}
