using UnityEngine;
using System.Collections.Generic;

public class Door : MonoBehaviour
{
    [Header("Door")]
    public KeyDoorMatchData match;
    [SerializeField] List<Collider> colliders;
    [SerializeField] List<Renderer> renderers;
    [SerializeField] List<SlidingTransition> transitions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialise the colors
        // 色を初期化する
        foreach (Renderer rdr in renderers)
        {
            rdr.material.color = match.color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        DoorManager.Instance.SetNearestDoor(this);
    }

    private void OnTriggerExit(Collider other)
    {
        DoorManager.Instance.SetNearestDoor(null);
    }

    public void UnlockDoor()
    {
        // Turn all the colliders off
        // すべてのコライダーをオフにする
        foreach (Collider cld in colliders)
        {
            cld.enabled = false;
        }

        // Do the slide transitions
        // スライドの切り替えを実行する
        foreach (SlidingTransition transition in transitions)
        {
            transition.DoTransition();
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Find References")]
    private void FindReferences()
    {
        // Add the colliders
        // collidersを追加する
        colliders.Clear();
        Collider[] colliderComponents = GetComponentsInChildren<Collider>();
        colliders = new(colliderComponents);

        // Add the renderers
        // レンダラーを追加する
        renderers.Clear();
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>();
        renderers = new(rendererComponents);

        // Add the transitions
        // トランジションを追加する
        transitions.Clear();
        SlidingTransition[] transitionComponents = GetComponentsInChildren<SlidingTransition>();
        transitions = new(transitionComponents);
    }
#endif
}
