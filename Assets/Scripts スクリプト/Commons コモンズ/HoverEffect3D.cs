using UnityEngine;

public class HoverEffect3D : MonoBehaviour
{
    [Header("Bobbing")]
    [SerializeField] float bobSpeed = 10f;
    [SerializeField] float bobHeight = 0.1f;
    [SerializeField] float bobSmooth = 10f;

    [Header("Rotation")]
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] bool shouldRotateClockwise = true;

    [Header("Hover Effect")]
    [SerializeField] Rigidbody rb;
    [SerializeField] bool isHovering = true;

    Vector3 startPosition;
    float bobTimer = 0f;

    private void Awake()
    {
        // Set the start position
        // 開始位置を設定する
        startPosition = transform.position;

        // Randomise the bobTimer so that different objects do not sync
        // bobTimerをランダム化して、異なるオブジェクトが同期しないようにする
        bobTimer = Random.Range(0f, Mathf.PI * 2f);

        // Hover if the bool is set to true at the start
        // 開始時にブール値がtrueに設定されている場合にホバーする
        if (isHovering)
        {
            StartHovering();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isHovering == false) return;
        Bob();
    }

    public void StartHovering()
    {
        Vector3 newVelocity = rb.angularVelocity;
        newVelocity.y = shouldRotateClockwise ? rotationSpeed : -rotationSpeed;
        rb.angularVelocity = newVelocity;

        isHovering = true;
    }

    public void StopHovering()
    {
        rb.angularVelocity = Vector3.zero;
        isHovering = false;
    }

    private void Bob()
    {
        bobTimer += Time.deltaTime;

        Vector3 newPosition = startPosition;
        newPosition.y += Mathf.Sin(bobSpeed * bobTimer) * bobHeight;

        transform.position = Vector3.Lerp(
            transform.position,
            newPosition,
            Time.deltaTime * bobSmooth
        );
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Assign the rb
        // rb を割り当てる
        rb = null;

        if (TryGetComponent<Rigidbody>(out rb) == false)
        {
            Debug.LogWarning("HoverEffect3D: Failed to find rigidbody component.");
        }
    }
#endif
}
