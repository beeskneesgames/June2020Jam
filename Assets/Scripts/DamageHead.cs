using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageHead {
    public static bool diagonalAllowed = false;
    public static bool preferHealthyCells = true;

    public Vector2Int coords;
    public CellInfo CurrentCellInfo {
        get {
            return Grid.Instance.CellInfoAt(coords);
        }
    }

    public Vector2Int Coords {
        get {
            return coords;
        }
        set {
            coords = value;
            CurrentCellInfo.Damage();
        }
    }

    public DamageHead(Vector2Int coords) {
        Coords = coords;
    }

    public void Move() {
        List<CellInfo> possibleCells = new List<CellInfo>();

        foreach (var cell in Grid.Instance.AdjacentTo(coords, diagonalAllowed)) {
            if (!cell.HasObstacle && !cell.HasPlayer) {
                possibleCells.Add(cell);
            }
        }

        if (preferHealthyCells) {
            List<CellInfo> healthyCells = new List<CellInfo>();

            foreach (var cell in possibleCells) {
                if (cell.IsHealthy) {
                    healthyCells.Add(cell);
                }
            }

            if (healthyCells.Count > 0) {
                possibleCells = healthyCells;
            }
        }

        if (possibleCells.Count > 0) {
            Vector2Int nextCoords = possibleCells[Random.Range(0, possibleCells.Count)].Coords;

            // If negative, the nextCoords is still set as its sentinel value,
            // which is effectively null here.
            if (nextCoords.x >= 0) {
                Coords = nextCoords;
            }
        }
    }
}
