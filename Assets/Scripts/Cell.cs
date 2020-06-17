using UnityEngine;

public class Cell : MonoBehaviour {
    public enum MouseState {
        None,
        Hovered,
        Selected
    }

    public CellInfo Info { get; private set; }
    public MouseState CurrentMouseState { get; set; } = MouseState.None;
    public bool inPath = false;

    private new Renderer renderer;
    private Color originalColor;

    private Material healthyMaterial;
    private Material damagedMaterial;
    private MaterialPropertyBlock damagedMaterialPropertyBlock;

    private Material currentMaterial;
    private Material CurrentMaterial {
        get {
            return currentMaterial;
        }

        set {
            if (value != currentMaterial) {
                currentMaterial = value;
                renderer.material = currentMaterial;
                renderer.SetPropertyBlock(null);
            }
        }
    }

    private void Awake() {
        Info = new CellInfo(this);
        damagedMaterialPropertyBlock = new MaterialPropertyBlock();
    }

    private void Start() {
        renderer = GetComponent<Renderer>();
        originalColor = renderer.material.GetColor("_BaseColor");

        healthyMaterial = MaterialDatabase.Instance.cellHealthy;
        damagedMaterial = MaterialDatabase.Instance.cellDamaged;
    }

    private void Update() {
        UpdateAppearance();
    }

    private void UpdateAppearance() {
        switch(CurrentMouseState) {
            case MouseState.None:
                if (inPath) {
                    // Light gray
                    CurrentMaterial = healthyMaterial;
                    renderer.material.SetColor("_BaseColor", new Color(0.75f, 0.75f, 0.75f));
                } else if (Info.HasBomb) {
                    // Black
                    CurrentMaterial = healthyMaterial;
                    renderer.material.SetColor("_BaseColor", Color.black);
                } else if (Info.HasObstacle) {
                    // Pink
                    CurrentMaterial = healthyMaterial;
                    renderer.material.SetColor("_BaseColor", new Color(1.0f, 0.5f, 1.00f));
                } else if (Info.HasDamageHead) {
                    // Dark red
                    CurrentMaterial = healthyMaterial;
                    renderer.material.SetColor("_BaseColor", new Color(1.0f, 0.0f, 0.0f));
                } else if (Info.IsDamaged) {
                    // Glitch
                    UseDamagedMaterial();
                } else {
                    // White
                    CurrentMaterial = healthyMaterial;
                    renderer.material.SetColor("_BaseColor", originalColor);
                }
                break;
            case MouseState.Hovered:
                CurrentMaterial = healthyMaterial;
                renderer.material.SetColor("_BaseColor", new Color(1.0f, 1.0f, 0.5f));
                break;
            case MouseState.Selected:
                CurrentMaterial = healthyMaterial;
                renderer.material.SetColor("_BaseColor", new Color(0.5f, 1.0f, 0.5f));
                break;
        }
    }

    private void UseDamagedMaterial() {
        CurrentMaterial = damagedMaterial;
        damagedMaterialPropertyBlock.SetVector("_Seed", new Vector4(Info.Coords.x, Info.Coords.y));
        renderer.SetPropertyBlock(damagedMaterialPropertyBlock);
    }
}
