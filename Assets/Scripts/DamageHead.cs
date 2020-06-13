using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageHead {
    public static bool diagonalAllowed = false;
    public static bool preferHealthyCells = true;

    public Vector2Int coords;
    private CellInfo CurrentCellInfo {
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
        List<CellInfo> possibleCells = Grid.Instance.AdjacentTo(coords, diagonalAllowed);
        List<CellInfo> availableCells = possibleCells.ToList();

        foreach (var cell in possibleCells) {
            if (cell.HasObstacle || cell.HasPlayer) {
                availableCells.Remove(cell);
            }
        }

        if (preferHealthyCells) {
            List<CellInfo> healthyCells = new List<CellInfo>();

            foreach (var cell in availableCells) {
                if (cell.IsHealthy) {
                    healthyCells.Add(cell);
                }
            }

            if (healthyCells.Count > 0) {
                possibleCells = healthyCells;
            }
        }

        if (availableCells.Count > 0) {
            Vector2Int nextCoords = availableCells[Random.Range(0, availableCells.Count)].Coords;

            // If negative, the nextCoords is still set as its sentinel value,
            // which is effectively null here.
            if (nextCoords.x >= 0) {
                Coords = nextCoords;
            }
        }
    }
}
