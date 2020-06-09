using UnityEngine;

public class DamageHead {
    public Vector2Int coords;

    public Vector2Int Coords {
        get {
            return coords;
        }
        set {
            coords = value;
            Grid.Instance.DamageCell(coords);
            GameManager.Instance.CheckEndGame();
        }
    }

    public DamageHead(Vector2Int coords) {
        Coords = coords;
    }

    public void Move() {
        CellInfo[] possibleCells = Grid.Instance.AdjacentTo(coords);

        if (possibleCells.Length > 0) {
            Vector2Int nextCoords = possibleCells[Random.Range(0, possibleCells.Length)].Coords;

            // If negative, the nextCoords is still set as its sentinel value,
            // which is effectively null here.
            if (nextCoords.x >= 0) {
                Coords = nextCoords;
            }
        }
    }
}
