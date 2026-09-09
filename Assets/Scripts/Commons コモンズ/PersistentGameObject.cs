using UnityEngine;

public sealed class PersistentGameObject : MonoBehaviour
{
    // Attach this script to Game Objects that need to be persistent.
    // 永続的に保持する必要のあるゲームオブジェクトに、このスクリプトを添付してください。

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
