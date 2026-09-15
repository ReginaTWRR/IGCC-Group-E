using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KeyDoorMatchDatabase", menuName = "Scriptable Objects/Doors/KeyDoorMatchDatabase")]
public class KeyDoorMatchDatabase : ScriptableObject
{
    [SerializeField] List<KeyDoorMatchData> keyDoorMatchDatas = new();

#if UNITY_EDITOR
    [ContextMenu("Find All Key Door Match Datas")]
    private void FindAllKeyDoorMatchDatas()
    {
        // Add the keyDoorMatchDatas
        // keyDoorMatchDatasを追加する

        keyDoorMatchDatas.Clear();
        string[] guids = AssetDatabase.FindAssets("t:KeyDoorMatchData");

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            KeyDoorMatchData data = AssetDatabase.LoadAssetAtPath<KeyDoorMatchData>(assetPath);

            if (data == null) continue;

            keyDoorMatchDatas.Add(data);
        }

        // Ask Unity to save this ScriptableObject
        // UnityにこのScriptableObjectを保存するように要求する
        EditorUtility.SetDirty(this);
    }
#endif
}
