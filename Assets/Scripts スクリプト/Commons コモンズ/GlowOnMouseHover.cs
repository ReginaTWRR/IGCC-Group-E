using UnityEngine;

public class GlowOnMouseHover : MonoBehaviour
{
    [Header("Glow Effect")]
    [SerializeField] GameObject objWithCollider;
    [SerializeField] Renderer rdr;
    [SerializeField] float glowIntensity = 4f;

    Color baseColor;

    bool isGlowing = false;
    public bool IsGlowing => isGlowing;

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

    // Update is called once per frame
    void Update()
    {
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
        float intensityFactor = Mathf.Pow(2, glowIntensity);

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
