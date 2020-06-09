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
        Coords = Grid.Instance.ChooseDamageableCoord(coords);
        Debug.Log(Coords);
    }
}
