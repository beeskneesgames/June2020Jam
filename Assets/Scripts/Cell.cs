using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    public Vector2Int Coords { get; set; }

    private new Renderer renderer;
    private Color originalColor;

    private void Start() {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    public void Highlight() {
        renderer.material.color = new Color(1.0f, 1.0f, 0.5f);
    }

    public void ClearHighlight() {
        renderer.material.color = originalColor;
    }
}
