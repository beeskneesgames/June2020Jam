using System.Collections.Generic;
using UnityEngine;

public class CellInfo {
    public Vector2Int Coords { get; set; } = new Vector2Int(-1, -1);

    public bool isDamaged = false;
    public bool IsDamaged {
        get {
            return isDamaged;
        }
        set {
            isDamaged = value;

            if (isDamaged && HasBomb) {
                // Remove damage on adjacent cells and remove bomb
            }

            GameManager.Instance.CheckEndGame();
        }
    }

    public bool IsHealthy {
        get {
            return !IsDamaged;
        }
    }

    public bool HasDamageHead { get; set; } = false;
    public bool HasObstacle { get; private set; } = false;
    public bool HasBomb { get; set; } = false;
    public bool HasPlayer {
        get {
            return Player.Instance.CurrentCoords == Coords;
        }
    }

    public void Damage() {
        IsDamaged = true;
    }

    public void Fix() {
        List<Vector2Int> path = Grid.PathBetween(Player.Instance.CurrentCell.Coords, Coords, Player.diagonalFixAllowed);

        Player.Instance.UseActionPoints(path.Count - 1);

        IsDamaged = false;

        if (HasDamageHead) {
            DamageManager.Instance.RemoveHeadsAt(Coords);
            HasDamageHead = false;
        }
    }

    public void AddBomb() {
        HasBomb = true;
        List<Vector2Int> path = Grid.PathBetween(Player.Instance.CurrentCell.Coords, Coords, Player.diagonalFixAllowed);

        Player.Instance.UseActionPoints(path.Count + 1);
    }

    public void AddObstacle() {
        HasObstacle = true;
    }

    public void RemoveObstacle() {
        HasObstacle = false;
    }
}
