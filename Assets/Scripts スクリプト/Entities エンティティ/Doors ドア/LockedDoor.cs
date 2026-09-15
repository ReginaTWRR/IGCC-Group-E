using UnityEngine;
using System.Collections.Generic;

public class LockedDoor
{
    public Door door;
    public List<Collider> colliders;

    public LockedDoor(Door door, List<Collider> colliders)
    {
        this.door = door;
        this.colliders = colliders;
    }
}
