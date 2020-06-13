using UnityEngine;

public class Cell : MonoBehaviour {
    public enum MouseState {
        None,
        Hovered,
        Selected
    }

    public CellInfo Info { get; set; } = new CellInfo();
    public MouseState CurrentMouseState { get; set; } = MouseState.None;
    public bool inPath = false;

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
                if (inPath) {
                    // Light gray
                    renderer.material.color = new Color(0.75f, 0.75f, 0.75f);
                } else if (Info.HasBomb) {
                    // Pink
                    renderer.material.color = new Color(0.0f, 0.0f, 0.0f);
                } else if (Info.HasObstacle) {
                    // Pink
                    renderer.material.color = new Color(1.0f, 0.5f, 1.00f);
                } else if (Info.HasDamageHead) {
                    // Dark red
                    renderer.material.color = new Color(1.0f, 0.0f, 0.0f);
                } else if (Info.IsDamaged) {
                    // Light red
                    renderer.material.color = new Color(1.0f, 0.5f, 0.5f);
                } else {
                    // Dark gray
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
