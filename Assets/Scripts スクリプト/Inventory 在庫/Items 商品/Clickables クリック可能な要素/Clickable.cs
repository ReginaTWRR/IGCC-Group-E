using UnityEngine;

public class Clickable : MonoBehaviour
{
    [Header("Clickable")]
    [SerializeField] ClickableItemInstance instance;
    [SerializeField] Renderer rdr;

    Color baseColor;
    bool isGlowing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseColor = rdr.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        Glow();
    }

    private void Glow()
    {
        if (isGlowing == false)
        {
            if (CursorManager.Instance.HoveredObject == this.gameObject &&
                CursorManager.Instance.IsCursorEnabled)
            {
                EnableGlow();
            }
        }
        else
        {
            if (CursorManager.Instance.HoveredObject != this.gameObject ||
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
        float intensityFactor = Mathf.Pow(2, instance.data.glowIntensity);

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
        rdr.material.EnableKeyword("_EMISSION");
        rdr.material.SetColor("_EmissionColor", newColor);

        // Update the isGlowing bool
        // 光る本を更新する
        isGlowing = true;
    }

    private void DisableGlow()
    {
        // Reset the color back to non-glowing
        // 色を元の非発光色に戻す
        rdr.material.DisableKeyword("_EMISSION");

        // Update the isGlowing bool
        // 光る本を更新する
        isGlowing = false;
    }
}
