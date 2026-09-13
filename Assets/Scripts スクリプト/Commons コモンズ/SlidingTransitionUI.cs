using UnityEngine;

public class SlidingTransitionUI : MonoBehaviour
{
    [Header("Sliding Transition")]
    [SerializeField] RectTransform tfm;
    [SerializeField] Vector3 hiddenPos;
    [SerializeField] Vector3 shownPos;
    [SerializeField] float slideDuration = 1f;
    [SerializeField] bool shouldShow = false;

    float slideTime = -Mathf.Infinity;

    public bool ShouldShow => shouldShow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tfm.localPosition = shouldShow ? shownPos : hiddenPos;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = shouldShow ? shownPos : hiddenPos;
        tfm.localPosition = Vector3.Lerp(
            tfm.localPosition,
            targetPos,
            (Time.time - slideTime) / slideDuration
        );
    }

    public void DoTransition()
    {
        slideTime = Time.time;
        shouldShow = !shouldShow;
    }
}
