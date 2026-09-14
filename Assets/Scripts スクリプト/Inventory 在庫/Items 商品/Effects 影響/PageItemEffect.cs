using UnityEngine;

[CreateAssetMenu(fileName = "PageItemEffect", menuName = "Scriptable Objects/Inventory/Items/Effects/PageItemEffect")]
public class PageItemEffect : ItemEffect
{
    public override void Use()
    {
        PageUI.Instance.ShowPage();
    }
}
