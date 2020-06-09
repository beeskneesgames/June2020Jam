using UnityEngine;

public class DamageHead {
    public Vector2Int coords;

    public Vector2Int Coords {
        get {
            return coords;
        }
        set {
            coords = value;
            GameManager.Instance.CheckEndGame();
        }
    }

    public DamageHead(Vector2Int coords) {
        Coords = coords;
    }

    public void Move() {
        Vector2Int[] possibleCoords = Grid.Instance.FindDamageableCoords(coords);
        Coords = possibleCoords[UnityEngine.Random.Range(0, possibleCoords.Length)];
    }
}
