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
        Vector2Int nextCoord = Grid.Instance.ChooseDamageableCoord(coords);

        // If negative, the nextCoord is still set as its sentinel value, which
        // is effectively null here.
        if (nextCoord.x >= 0) {
            Coords = nextCoord;
        }
    }
}
