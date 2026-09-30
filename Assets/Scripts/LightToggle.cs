using UnityEngine;

/// Enciende/apaga una luz y cambia el color de la bombilla. Se llama desde un XR Simple Interactable (rayo).
public class LightToggle : MonoBehaviour
{
    public Light targetLight;
    public Renderer bulbRenderer;
    public Color onColor = new Color(1f, 0.9f, 0.4f);
    public Color offColor = new Color(0.2f, 0.2f, 0.2f);

    void Start() { Refresh(); }

    public void Toggle()
    {
        if (targetLight == null) return;
        targetLight.enabled = !targetLight.enabled;
        Refresh();
    }

    void Refresh()
    {
        if (bulbRenderer == null || targetLight == null) return;
        var c = targetLight.enabled ? onColor : offColor;
        bulbRenderer.material.color = c;
        if (bulbRenderer.material.HasProperty("_EmissionColor"))
        {
            bulbRenderer.material.EnableKeyword("_EMISSION");
            bulbRenderer.material.SetColor("_EmissionColor", targetLight.enabled ? c * 2f : Color.black);
        }
    }
}
