using UnityEngine;

public class TimerUI : PersistentSingleton<TimerUI>
{
    [Header("Timer")]
    [SerializeField] GameObject clock;
    [SerializeField] Transform pivotTransform;

    float timer = 0f;
    float duration = 0f;

    public bool IsTimerRunning => (timer > 0f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clock.SetActive(IsTimerRunning);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTimerRunning == false) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SetTimer(0f);
        }

        // Calculate the UI display
        // UI表示を計算する
        float zRotation = ((timer / duration) * 360f) + 90f;
        int intTimer = (int)(timer + 0.5f); // Round up 切り上げする

        pivotTransform.rotation = Quaternion.Euler(0f, 0f, zRotation);
    }

    public void SetTimer(float duration)
    {
        this.duration = duration;
        timer = duration;
        clock.SetActive(IsTimerRunning);
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the clock
        // 時計を追加する
        clock = null;
        Transform clockTransform = transform.GetChild(0);
        clock = clockTransform.gameObject;

        if (clock == null)
        {
            Debug.LogWarning("TimerUI: Failed to find clock object.");
        }

        // Add the pivotTransform
        // pivotTransformを追加する
        pivotTransform = null;
        pivotTransform = clockTransform.GetChild(0);

        if (pivotTransform == null)
        {
            Debug.LogWarning("TimerUI: Failed to find pivot transform component.");
        }
    }
#endif
}
