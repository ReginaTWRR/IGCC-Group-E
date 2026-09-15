using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct TransformReaction
{
    public GameObject receiver;
    public bool isEnabledWhenGhost;

    public TransformReaction(GameObject receiver, bool isEnabledWhenGhost)
    {
        this.receiver = receiver;
        this.isEnabledWhenGhost = isEnabledWhenGhost;
    }
}

public class TransformationMonitor : PersistentSingleton<TransformationMonitor>
{
    [Header("Transformation Monitor")]
    [SerializeField] List<TransformReaction> reactions;

    bool isPlayerAGhost = false;
    public bool IsPlayerAGhost => isPlayerAGhost;

    public void React(bool isPlayerAGhost)
    {
        // React accordingly
        // 状況に応じて対応します
        this.isPlayerAGhost = isPlayerAGhost;

        foreach (TransformReaction reaction in reactions)
        {
            if (isPlayerAGhost)
            {
                reaction.receiver.SetActive(reaction.isEnabledWhenGhost);
            }
            else
            {
                reaction.receiver.SetActive(!reaction.isEnabledWhenGhost);
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        reactions.Clear();

        // Add the invSystem
        // invSystemを追加する
        GameObject invSystem = GameObject.Find("Inventory System");

        if (invSystem != null)
        {
            reactions.Add(new(invSystem, false));
        }
        else
        {
            Debug.LogWarning("TransformReactionManager: Failed to find inv system object.");
        }

        // Add the invCanvas
        // invCanvasを追加する
        GameObject invCanvas = GameObject.Find("Inventory Canvas");

        if (invCanvas != null)
        {
            reactions.Add(new(invCanvas, false));
        }
        else
        {
            Debug.LogWarning("TransformReactionManager: Failed to find inv canvas object.");
        }

        // Add the collectibles
        // 収集品を追加する
        CollectibleItemInstance[] collectibles = Object.FindObjectsByType<CollectibleItemInstance>(FindObjectsSortMode.None);

        foreach (CollectibleItemInstance collectible in collectibles)
        {
            reactions.Add(new(collectible.gameObject, false));
        }
    }
#endif
}
