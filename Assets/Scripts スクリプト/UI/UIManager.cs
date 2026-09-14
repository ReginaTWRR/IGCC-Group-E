using UnityEngine;

public class UIManager : PersistentSingleton<UIManager>
{
    [Header("UI Manager")]
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject page;
    [SerializeField] GameObject timer;

    public void SetDefault()
    {
        inventory.SetActive(true);
        page.SetActive(false);
        timer.SetActive(true);
    }

    public void FocusPage()
    {
        inventory.SetActive(false);
        page.SetActive(true);
        timer.SetActive(false);
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the inventory
        // 在庫を追加する
        inventory = null;
        Transform invTransform = transform.Find("Inventory Canvas");
        inventory = invTransform.gameObject;

        if (inventory == null)
        {
            Debug.LogWarning("UIManager: Failed to find inventory object.");
        }

        // Add the page
        // ページを追加する
        page = null;
        Transform pageTransform = transform.Find("Page Canvas");
        page = pageTransform.gameObject;

        if (page == null)
        {
            Debug.LogWarning("UIManager: Failed to find page object.");
        }

        // Add the timer
        // タイマーを追加
        timer = null;
        Transform tmrTransform = transform.Find("Timer Canvas");
        timer = tmrTransform.gameObject;

        if (timer == null)
        {
            Debug.LogWarning("UIManager: Failed to find timer object.");
        }
    }
#endif
}
