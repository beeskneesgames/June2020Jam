using UnityEngine;

public class CellInfo {
    public Vector2Int Coords { get; set; }

    public CellInfo() {
        Coords = new Vector2Int(-1, -1);
    }
}
