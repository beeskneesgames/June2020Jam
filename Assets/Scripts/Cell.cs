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
        originalColor = renderer.material.GetColor("_BaseColor");
    }

    private void Update() {
        UpdateAppearance();
    }

    private void UpdateAppearance() {
        switch(CurrentMouseState) {
            case MouseState.None:
                if (inPath) {
                    // Light gray
                    renderer.material.SetColor("_BaseColor", new Color(0.75f, 0.75f, 0.75f));
                } else if (Info.HasBomb) {
                    // Black
                    renderer.material.SetColor("_BaseColor", Color.black);
                } else if (Info.HasObstacle) {
                    // Pink
                    renderer.material.SetColor("_BaseColor", new Color(1.0f, 0.5f, 1.00f));
                } else if (Info.HasDamageHead) {
                    // Dark red
                    renderer.material.SetColor("_BaseColor", new Color(1.0f, 0.0f, 0.0f));
                } else if (Info.IsDamaged) {
                    // Light red
                    renderer.material.SetColor("_BaseColor", new Color(1.0f, 0.5f, 0.5f));
                } else {
                    // White
                    renderer.material.SetColor("_BaseColor", originalColor);
                }
                break;
            case MouseState.Hovered:
                renderer.material.SetColor("_BaseColor", new Color(1.0f, 1.0f, 0.5f));
                break;
            case MouseState.Selected:
                renderer.material.SetColor("_BaseColor", new Color(0.5f, 1.0f, 0.5f));
                break;
        }
    }
}
