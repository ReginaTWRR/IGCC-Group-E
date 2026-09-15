using UnityEngine;

[CreateAssetMenu(fileName = "KeyItemEffect", menuName = "Scriptable Objects/Items/Effects/KeyItemEffect")]
public class KeyItemEffect : ItemEffect
{
    [Header("Key Item")]
    [SerializeField] KeyDoorMatchData match;

    public override void TriggerEffect()
    {
        DoorManager.Instance.TryUnlockDoor(match);
    }
}
