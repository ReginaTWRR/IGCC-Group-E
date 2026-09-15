using UnityEngine;

public class SlidingTransition : MonoBehaviour
{
    [Header("Sliding Transition")]
    [SerializeField] Transform tfm;
    [SerializeField] Vector3 hiddenPos;
    [SerializeField] Vector3 shownPos;
    [SerializeField] float slideDuration = 1f;
    [SerializeField] bool shouldShow = false;
    [SerializeField] bool shouldCopyHiddenPos = false;
    [SerializeField] bool shouldCopyShownPos = false;

    float slideTime = -Mathf.Infinity;

    public bool ShouldShow => shouldShow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (shouldCopyHiddenPos)
        {
            hiddenPos = tfm.localPosition;
        }

        if (shouldCopyShownPos)
        {
            shownPos = tfm.localPosition;
        }

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
