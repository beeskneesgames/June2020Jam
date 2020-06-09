using UnityEngine;

public class CellInfo {
    public Vector2Int Coords { get; set; }

    public bool isDamaged = false;
    public bool IsDamaged {
        get {
            return isDamaged;
        }
        set {
            isDamaged = value;
            GameManager.Instance.CheckEndGame();
        }
    }

    public CellInfo() {
        Coords = new Vector2Int(-1, -1);
    }
}
