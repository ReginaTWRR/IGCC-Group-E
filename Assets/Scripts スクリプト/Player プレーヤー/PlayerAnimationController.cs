using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Animation Controller")]
    [SerializeField] Animator animator;

    Player plr;

    // Animation hashes
    // アニメーションハッシュ
    int isStartingWalkHash = Animator.StringToHash("isStartingWalk");
    int isStoppingWalkHash = Animator.StringToHash("isStoppingWalk");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Cache the PlayerManager script
        // PlayerManagerスクリプトをキャッシュする
        plr = Player.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(isStartingWalkHash, plr.IsStartingWalk);
        animator.SetBool(isStoppingWalkHash, plr.IsStoppingWalk);

        DebugLogs();
    }

    private void DebugLogs()
    {
        // Print the current animation clip
        // 現在のアニメーションクリップを表示する
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        Debug.Log(clipInfo[0].clip.name);
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the animator
        // アニメーターを追加する
        animator = null;
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("PlayerAnimationController: Failed to find animator component.");
        }
    }
#endif
}
