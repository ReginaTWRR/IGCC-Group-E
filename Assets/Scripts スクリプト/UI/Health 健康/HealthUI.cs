using UnityEngine;

public class HealthUI : PersistentSingleton<HealthUI>
{
    [Header("Health")]
    [SerializeField] RectTransform fillTransform;
    [SerializeField] float transitionSpeed = 10f;

    Vector3 startingPosition;

    float maxHealth;
    float maxWidth;

    float targetWidth;
    Vector3 targetPosition;

    protected override void Awake()
    {
        base.Awake();

        maxWidth = fillTransform.rect.width;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPosition = fillTransform.localPosition;

        maxHealth = Player.Instance.MaxHealth;
        
        float currentHealth = Player.Instance.CurrentHealth;
        UpdateHealthUI(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        float currentWidth = fillTransform.sizeDelta.x;

        float newWidth = Mathf.Lerp(
            currentWidth,
            targetWidth,
            transitionSpeed * Time.deltaTime
        );
        Vector3 newPosition = Vector3.Lerp(
            fillTransform.localPosition,
            targetPosition,
            transitionSpeed * Time.deltaTime
        );

        SetFillWidth(newWidth);
        SetPosition(newPosition);
    }

    public void UpdateHealthUI(float currentHealth)
    {
        targetWidth = (currentHealth / maxHealth) * maxWidth;

        float xOffset = (maxWidth - targetWidth) * 0.5f;
        targetPosition = startingPosition;
        targetPosition.x -= xOffset;
    }

    private void SetFillWidth(float fillWidth)
    {
        Vector2 size = fillTransform.sizeDelta;
        size.x = fillWidth;
        fillTransform.sizeDelta = size;
    }

    private void SetPosition(Vector3 position)
    {
        fillTransform.localPosition = position;
    }
}
