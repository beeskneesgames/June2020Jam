using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    public enum MouseState {
        None,
        Hovered,
        Selected
    }

    public MouseState CurrentMouseState { get; set; } = MouseState.None;
    public Vector2Int Coords { get; set; }

    private new Renderer renderer;
    private Color originalColor;

    public bool isDamaged;
    public bool IsDamaged {
        get {
            return isDamaged;
        }
        set {
            isDamaged = value;
            GameManager.Instance.CheckEndGame();
        }
    }

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
                if (IsDamaged) {
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
