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

    private Material currentMaterial;
    private Material CurrentMaterial {
        get {
            return currentMaterial;
        }

        set {
            if (value != currentMaterial) {
                currentMaterial = value;
                renderer.material = currentMaterial;
            }
        }
    }

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
                    CurrentMaterial = MaterialDatabase.Instance.cellHealthy;
                    renderer.material.SetColor("_BaseColor", new Color(0.75f, 0.75f, 0.75f));
                } else if (Info.HasBomb) {
                    // Black
                    CurrentMaterial = MaterialDatabase.Instance.cellHealthy;
                    renderer.material.SetColor("_BaseColor", Color.black);
                } else if (Info.HasObstacle) {
                    // Pink
                    CurrentMaterial = MaterialDatabase.Instance.cellHealthy;
                    renderer.material.SetColor("_BaseColor", new Color(1.0f, 0.5f, 1.00f));
                } else if (Info.HasDamageHead) {
                    // Dark red
                    CurrentMaterial = MaterialDatabase.Instance.cellHealthy;
                    renderer.material.SetColor("_BaseColor", new Color(1.0f, 0.0f, 0.0f));
                } else if (Info.IsDamaged) {
                    // Glitch
                    CurrentMaterial = MaterialDatabase.Instance.cellDamaged;
                } else {
                    // White
                    CurrentMaterial = MaterialDatabase.Instance.cellHealthy;
                    renderer.material.SetColor("_BaseColor", originalColor);
                }
                break;
            case MouseState.Hovered:
                CurrentMaterial = MaterialDatabase.Instance.cellHealthy;
                renderer.material.SetColor("_BaseColor", new Color(1.0f, 1.0f, 0.5f));
                break;
            case MouseState.Selected:
                CurrentMaterial = MaterialDatabase.Instance.cellHealthy;
                renderer.material.SetColor("_BaseColor", new Color(0.5f, 1.0f, 0.5f));
                break;
        }
    }
}
