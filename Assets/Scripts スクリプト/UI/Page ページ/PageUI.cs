using UnityEngine;
using UnityEngine.UI;

public class PageUI : PersistentSingleton<PageUI>
{
    [Header("Page")]
    [SerializeField] GameObject page;
    [SerializeField] Image pageImage;
    [SerializeField] bool isPageShown = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        page.SetActive(isPageShown);
    }

    public void ShowPage(Sprite pageSprite)
    {
        GameManager.Instance.PauseGame();
        UIManager.Instance.FocusPage();
        CursorManager.Instance.EnableCursor();

        pageImage.sprite = pageSprite;

        isPageShown = true;
        page.SetActive(isPageShown);
    }

    public void HidePage()
    {
        GameManager.Instance.ResumeGame();
        UIManager.Instance.SetDefault();
        CursorManager.Instance.DisableCursor();

        pageImage.sprite = null;

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

        // Add the page image
        // ページ画像を追加する
        pageImage = null;

        if (pageTransform.TryGetComponent<Image>(out pageImage) == false)
        {
            Debug.LogWarning("PageUI: Failed to find image component.");
        }
    }
#endif
}
