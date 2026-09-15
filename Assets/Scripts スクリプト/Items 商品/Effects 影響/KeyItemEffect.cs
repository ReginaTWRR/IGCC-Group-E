using UnityEngine;

[CreateAssetMenu(fileName = "KeyItemEffect", menuName = "Scriptable Objects/Items/Effects/KeyItemEffect")]
public class KeyItemEffect : ItemEffect
{
    [Header("Key Item")]
    [SerializeField] KeyDoorMatchData match;
    public KeyDoorMatchData Match => match;

    public override bool TriggerEffect()
    {
        return DoorManager.Instance.TryUnlockDoor(match);
    }
}
