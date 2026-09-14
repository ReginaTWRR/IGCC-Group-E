using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CollectibleItemDatabase", menuName = "Scriptable Objects/Inventory/Items/CollectibleItemDatabase")]
public class CollectibleItemDatabase : ScriptableObject
{
    [Header("Collectible Item Database")]
    [SerializeField] List<CollectibleItemData> collectibleItemDatas = new();

    public CollectibleItemData GetByItemSprite(Sprite itemSprite)
    {
        foreach (CollectibleItemData item in collectibleItemDatas)
        {
            if (item.itemSprite == itemSprite) return item;
        }

        return null;
    }

#if UNITY_EDITOR
    [ContextMenu("Find All Collectible Item Datas")]
    private void FindAllCollectibleItemDatas()
    {
        // Add the collectible item datas
        // 収集アイテムのデータを追加する
        collectibleItemDatas.Clear();
        string[] guids = AssetDatabase.FindAssets("t:CollectibleItemData");

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            CollectibleItemData data = AssetDatabase.LoadAssetAtPath<CollectibleItemData>(assetPath);

            if (data == null) continue;

            collectibleItemDatas.Add(data);
        }

        // Ask Unity to save this ScriptableObject
        // UnityにこのScriptableObjectを保存するように要求する
        EditorUtility.SetDirty(this);
    }
#endif
}
