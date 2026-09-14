using UnityEngine;

public class PageUI : PersistentSingleton<PageUI>
{
    [Header("Page")]
    [SerializeField] GameObject page;
    [SerializeField] bool isPageShown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        page.SetActive(isPageShown);
    }

    public void ShowPage()
    {
        GameManager.Instance.PauseGame();
        UIManager.Instance.FocusPage();
        CursorManager.Instance.EnableCursor();

        isPageShown = true;
        page.SetActive(isPageShown);
    }

    public void HidePage()
    {
        GameManager.Instance.ResumeGame();
        UIManager.Instance.SetDefault();
        CursorManager.Instance.DisableCursor();

        isPageShown = false;
        page.SetActive(isPageShown);
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the page
        // ページを追加する
        page = null;
        Transform pageTransform = transform.GetChild(0);
        page = pageTransform.gameObject;

        if (page == null)
        {
            Debug.LogWarning("PageUI: Failed to find page object.");
        }
    }
#endif
}
