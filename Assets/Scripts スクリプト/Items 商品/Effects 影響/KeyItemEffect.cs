using UnityEngine;

[CreateAssetMenu(fileName = "KeyItemEffect", menuName = "Scriptable Objects/Items/Effects/KeyItemEffect")]
public class KeyItemEffect : ItemEffect
{
    public override void TriggerEffect()
    {
        DoorManager.Instance.TryUnlockDoor();
    }
}
