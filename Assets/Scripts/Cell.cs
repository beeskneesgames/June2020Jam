using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    public enum MouseState {
        None,
        Hovered,
        Selected
    }

    public CellInfo Info { get; set; } = new CellInfo();
    public MouseState CurrentMouseState { get; set; } = MouseState.None;

    private new Renderer renderer;
    private Color originalColor;

    private void Start() {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.color;
    }

    private void Update() {
        UpdateAppearance();
    }

    private void UpdateAppearance() {
        switch(CurrentMouseState) {
            case MouseState.None:
                if (Info.IsDamageHead) {
                    renderer.material.color = new Color(1.0f, 0.0f, 0.0f);
                } else if (Info.IsDamaged) {
                    renderer.material.color = new Color(1.0f, 0.5f, 0.5f);
                } else {
                    renderer.material.color = originalColor;
                }
                break;
            case MouseState.Hovered:
                renderer.material.color = new Color(1.0f, 1.0f, 0.5f);
                break;
            case MouseState.Selected:
                renderer.material.color = new Color(0.5f, 1.0f, 0.5f);
                break;
        }
    }
}
