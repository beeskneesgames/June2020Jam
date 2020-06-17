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
    public bool inActionPath = false;

    private new Renderer renderer;
    private Color originalColor;
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
    }

    private void Update() {
        UpdateAppearance();
    }

    private void UpdateAppearance() {
        switch(CurrentMouseState) {
            case MouseState.None:
                if (inPath) {
                    // Light gray
                    // TODO show damage thru path highlight
                    CurrentMaterial = MaterialDatabase.Instance.cellInPath;
                } else if (inActionPath) {
                    // Lighter gray
                    // TODO show damage thru area highlight
                    CurrentMaterial = MaterialDatabase.Instance.cellInActionArea;
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
                    UseDamagedMaterial();
                } else {
                    // Blue #42676E (0.259, 0.404, 0.431)
                    CurrentMaterial = MaterialDatabase.Instance.cellHealthy;
                    renderer.material.SetColor("_BaseColor", originalColor);
                }
                break;
            case MouseState.Hovered:
                CurrentMaterial = MaterialDatabase.Instance.cellHover;
                break;
            case MouseState.Selected:
                CurrentMaterial = MaterialDatabase.Instance.cellHealthy;
                renderer.material.SetColor("_BaseColor", new Color(0.5f, 1.0f, 0.5f));
                break;
        }
    }

    private void UseDamagedMaterial() {
        CurrentMaterial = MaterialDatabase.Instance.cellDamaged;
        damagedMaterialPropertyBlock.SetVector("_Seed", new Vector4(Info.Coords.x, Info.Coords.y));
        renderer.SetPropertyBlock(damagedMaterialPropertyBlock);
    }
}
