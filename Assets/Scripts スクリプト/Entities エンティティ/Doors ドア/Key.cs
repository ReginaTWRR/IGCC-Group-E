using UnityEngine;

public class Key : MonoBehaviour
{
    [Header("Key")]
    [SerializeField] KeyItemEffect effect;
    [SerializeField] Renderer rdr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialise the color
        // 色を初期化する
        rdr.material.color = effect.Match.color;
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the renderer
        // レンダラーを追加する
        rdr = null;
        rdr = GetComponentInChildren<Renderer>();

        if (rdr == null)
        {
            Debug.LogWarning("Key: Failed to find renderer component.");
        }
    }
#endif
}
