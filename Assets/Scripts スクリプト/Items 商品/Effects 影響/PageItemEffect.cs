using UnityEngine;

[CreateAssetMenu(fileName = "PageItemEffect", menuName = "Scriptable Objects/Items/Effects/PageItemEffect")]
public class PageItemEffect : ItemEffect
{
    [Header("Page Item Effect")]
    public Sprite pageSprite;

    public override bool TriggerEffect()
    {
        PageUI.Instance.ShowPage(pageSprite);
        return true;
    }
}
