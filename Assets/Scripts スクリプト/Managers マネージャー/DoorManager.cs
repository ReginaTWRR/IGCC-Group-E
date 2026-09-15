using UnityEngine;

public class DoorManager : PersistentSingleton<DoorManager>
{
    Door nearestDoor;

    public void SetNearestDoor(Door nearestDoor)
    {
        this.nearestDoor = nearestDoor;
    }

    public bool TryUnlockDoor(KeyDoorMatchData match)
    {
        if (nearestDoor == null) return false;
        if (nearestDoor.match.door != match.door) return false;

        nearestDoor.UnlockDoor();

        return true;
    }
}
