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
            GameManager.Instance.CheckEndGame();
        }
    }

    public bool HasDamageHead { get; set; } = false;

    public void Damage() {
        IsDamaged = true;
    }

    public void Fix() {
        Player.Instance.UseActionPoints(1);
        IsDamaged = false;

        if (HasDamageHead) {
            DamageManager.Instance.RemoveHeadsAt(Coords);
            HasDamageHead = false;
        }
    }
}
