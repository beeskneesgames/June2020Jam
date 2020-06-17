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
            GameManager.Instance.StateChanged();
        }
    }

    public const int BombCost = 1;
    public const int MeleeFixCost = 1;
    public const int RangedFixCost = 1;
    public const int RangedFixRange = 5;

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

    private Cell cell;

    public CellInfo(Cell c) {
        cell = c;
    }

    public void Damage() {
        IsDamaged = true;
    }

    public void MeleeFix() {
        foreach (var cell in Grid.Instance.AdjacentTo(Player.Instance.CurrentCell.Coords, true)) {
            cell.Fix();
        }

        Player.Instance.UseActionPoints(MeleeFixCost);
    }

    public void RangedFix() {
        Fix();
        Player.Instance.UseActionPoints(RangedFixCost);
    }

    public void AddBomb() {
        HasBomb = true;
        Player.Instance.UseActionPoints(BombCost);
    }

    public void AddObstacle() {
        HasObstacle = true;
    }

    public void RemoveObstacle() {
        HasObstacle = false;
    }

    private void Fix() {
        IsDamaged = false;

        if (HasDamageHead) {
            DamageManager.Instance.RemoveHeadsAt(Coords);
            HasDamageHead = false;
        }
    }
}
