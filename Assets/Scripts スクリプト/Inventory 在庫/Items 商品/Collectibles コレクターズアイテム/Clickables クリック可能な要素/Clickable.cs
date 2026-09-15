using UnityEngine;

public class Clickable : Collectible
{
    [Header("Clickable")]
    [SerializeField] GameObject objWithCollider;
    [SerializeField] Renderer rdr;

    // Cache the clickable item data if the type casting was successful
    // 型変換が成功した場合、クリック可能なアイテムのデータをキャッシュする
    ClickableItemData data;

    Color baseColor;
    bool isGlowing = false;

    protected override void CheckCollection()
    {
        // Check if the item has been clicked while the mouse is hovered over it
        // マウスカーソルがアイテムの上に重なっている間に、アイテムがクリックされたかどうかを確認します。
        if (isGlowing && InventoryInputHandler.Instance.CheckCollectItemPressed())
        {
            CollectItem();
        }
    }

    private void Awake()
    {
        if (instance.data is ClickableItemData clickableData)
        {
            data = clickableData;
        }
        else
        {
            Debug.LogError("Clickable: The instance has an invalid itemData.");
            Debug.Log($"Clickable: itemData = {instance.data}");
            Debug.Log($"Clickable: gameObject = {gameObject.name}");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rdr.material.HasProperty("_Color"))
        {
            baseColor = rdr.material.GetColor("_Color");
        }
        // If the previous check failed, we check for the color based on the ArnoldStandardSurface shader
        // 前のチェックが失敗した場合、ArnoldStandardSurface シェーダーに基づいて色をチェックします
        else if (rdr.material.HasProperty("_BASE_COLOR"))
        {
            baseColor = rdr.material.GetColor("_BASE_COLOR");
        }
    }

    protected override void Update()
    {
        base.Update();
        Glow();
    }

    private void Glow()
    {
        // Detect mouse hover with the object that has the collider component
        // コライダーコンポーネントを持つオブジェクトでマウスオーバーを検出します
        if (isGlowing == false)
        {
            if (CursorManager.Instance.HoveredObject == objWithCollider &&
                CursorManager.Instance.IsCursorEnabled)
            {
                EnableGlow();
            }
        }
        else
        {
            if (CursorManager.Instance.HoveredObject != objWithCollider ||
                CursorManager.Instance.IsCursorEnabled == false)
            {
                DisableGlow();
            }
        }
    }

    private void EnableGlow()
    {
        // Calculate the intensity factor using 2 to the power of glowIntensity
        // glowIntensityの2乗を使用して強度係数を計算します
        float intensityFactor = Mathf.Pow(2, data.glowIntensity);

        // Create the newColor
        // 新しい色を作成する
        Color newColor = new(
            baseColor.r * intensityFactor,
            baseColor.g * intensityFactor,
            baseColor.b * intensityFactor,
            baseColor.a
        );

        // Apply the new color to the material
        // マテリアルに新しい色を適用する
        if (rdr.material.HasProperty("_EmissionColor"))
        {
            rdr.material.EnableKeyword("_EMISSION");
            rdr.material.SetColor("_EmissionColor", newColor);
        }
        else if (rdr.material.HasProperty("_EMISSION_COLOR"))
        {
            rdr.material.SetColor("_EMISSION_COLOR", newColor);
        }

        // Update the isGlowing bool
        // 光る本を更新する
        isGlowing = true;
    }

    private void DisableGlow()
    {
        // Reset the color back to non-glowing
        // 色を元の非発光色に戻す
        if (rdr.material.HasProperty("_EmissionColor"))
        {
            rdr.material.SetColor("_EmissionColor", Color.black);
            rdr.material.DisableKeyword("_EMISSION");
        }
        else if (rdr.material.HasProperty("_EMISSION_COLOR"))
        {
            rdr.material.SetColor("_EMISSION_COLOR", Color.black);
        }

        // Update the isGlowing bool
        // 光る本を更新する
        isGlowing = false;
    }
}
