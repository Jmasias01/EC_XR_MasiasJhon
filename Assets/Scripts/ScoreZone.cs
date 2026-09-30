using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// Contador: suma un punto cada vez que un objeto con Rigidbody entra en la canasta.
[RequireComponent(typeof(Collider))]
public class ScoreZone : MonoBehaviour
{
    public Text scoreText;
    int score;
    readonly HashSet<Rigidbody> inside = new HashSet<Rigidbody>();

    void Start() { UpdateText(); }

    void OnTriggerEnter(Collider other)
    {
        var rb = other.attachedRigidbody;
        if (rb == null || inside.Contains(rb)) return;
        inside.Add(rb);
        score++;
        UpdateText();
    }

    void OnTriggerExit(Collider other)
    {
        var rb = other.attachedRigidbody;
        if (rb != null) inside.Remove(rb);
    }

    public void ResetScore() { score = 0; UpdateText(); }

    void UpdateText()
    {
        if (scoreText != null) scoreText.text = "Puntos: " + score;
    }
}
