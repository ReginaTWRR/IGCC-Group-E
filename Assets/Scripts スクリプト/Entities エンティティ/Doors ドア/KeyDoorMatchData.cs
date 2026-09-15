using UnityEngine;

[CreateAssetMenu(fileName = "KeyDoorMatchData", menuName = "Scriptable Objects/Doors/KeyDoorMatchData")]
public class KeyDoorMatchData : ScriptableObject
{
    [Header("Key Door Match")]
    public KeyData key;
    public DoorData door;
    public Color color = Color.white;
}
