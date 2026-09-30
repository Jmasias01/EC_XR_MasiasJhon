using UnityEngine;

/// Aparece una copia de un objeto agarrable (clona sus componentes Rigidbody y XR Grab Interactable).
public class ObjectSpawner : MonoBehaviour
{
    public GameObject template;
    public Transform spawnPoint;
    int count;

    public void Spawn()
    {
        if (template == null || spawnPoint == null) return;
        var go = Instantiate(template, spawnPoint.position, Quaternion.identity);
        go.name = template.name + "_Clon" + (++count);
        go.SetActive(true);
        var r = go.GetComponent<Renderer>();
        if (r != null) r.material.color = Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.8f, 1f);
    }
}
