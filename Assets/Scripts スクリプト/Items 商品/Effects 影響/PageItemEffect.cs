using UnityEngine;

[CreateAssetMenu(fileName = "PageItemEffect", menuName = "Scriptable Objects/Items/Effects/PageItemEffect")]
public class PageItemEffect : ItemEffect
{
    [Header("Page Item Effect")]
    public Sprite pageSprite;

    public override void TriggerEffect()
    {
        PageUI.Instance.ShowPage(pageSprite);
    }
}
