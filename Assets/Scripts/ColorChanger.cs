using UnityEngine;

/// Cambia el color de un objeto en ciclo. Se llama desde un botón de UI espacial (rayo).
public class ColorChanger : MonoBehaviour
{
    public Renderer target;
    public Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta };
    int index;

    public void NextColor()
    {
        if (target == null || colors.Length == 0) return;
        index = (index + 1) % colors.Length;
        target.material.color = colors[index];
    }
}
