using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour {
    public int spreadRate = 3;
    public GameObject damageHeadPrefab;

    private List<DamageHead> damageHeads;
    private Vector2Int DefaultHeadCoords1;
    private Vector2Int DefaultHeadCoords2;
    private Vector2Int DefaultHeadCoords3;
    private const int RandomDamageHeadRate = 1;

    private static DamageManager instance;
    public static DamageManager Instance {
        get {
            return instance;
        }
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start() {
        Reset();
    }

    public void Reset() {
        DefaultHeadCoords1 = new Vector2Int(0, 0);
        DefaultHeadCoords2 = new Vector2Int(0, Grid.Instance.Size.y - 1);
        DefaultHeadCoords3 = new Vector2Int(Grid.Instance.Size.x - 1, 0);
        damageHeads = new List<DamageHead>();

        AddHead(DefaultHeadCoords1);
        AddHead(DefaultHeadCoords2);
        AddHead(DefaultHeadCoords3);

        Grid.Instance.SetDamageHeads(damageHeads);
    }

    public void Spread() {
        if ((Turn.Instance.TurnCount % spreadRate) == 0) {
            // Divide current heads
            if (damageHeads.Count > 0) {
                foreach (var head in new List<DamageHead>(damageHeads)) {
                    AddHead(head.Coords);
                }
            } else {
                Reset();
            }

            // Add random new heads
            List<CellInfo> cells = new List<CellInfo>();
            foreach (var cell in Grid.Instance.DamagedCells) {
                if (!cell.HasDamageHead) {
                    cells.Add(cell);
                }
            }
            for (int i = 0; i < RandomDamageHeadRate; i++) {
                AddHead(cells[Random.Range(0, cells.Count)].Coords);
            }
        }

        foreach (DamageHead damageHead in damageHeads) {
            damageHead.Move();
        }

        ExplodeBombs();

        Grid.Instance.SetDamageHeads(damageHeads);
    }

    public void RemoveHeadsAt(Vector2Int coords) {
        List<DamageHead> damageHeadsToDestroy = new List<DamageHead>();

        foreach (var damageHead in damageHeads) {
            if (damageHead.Coords == coords) {
                damageHeadsToDestroy.Add(damageHead);
            }
        }

        foreach (var damageHead in damageHeadsToDestroy) {
            damageHeads.Remove(damageHead);
            damageHead.DestroyGameObject();
        }
    }

    private void AddHead(Vector2Int coords) {
        damageHeads.Add(new DamageHead(coords, Instantiate(damageHeadPrefab)));
    }

    // TODO: Move this logic into some sort of Bomb or BombManager class.
    private void ExplodeBombs() {
        foreach (var cell in Grid.Instance.AllCells) {
            if (cell.IsDamaged && cell.HasBomb) {
                // Remove damage on adjacent cells and remove bomb
                List<DamageHead> newDamageHeads = new List<DamageHead>();
                List<CellInfo> cellsToFix = Grid.Instance.AdjacentTo(cell.Coords, true);
                cellsToFix.Add(Grid.Instance.CellInfoAt(cell.Coords));

                foreach (var cellToFix in cellsToFix) {
                    if (cellToFix.HasDamageHead) {
                        RemoveHeadsAt(cellToFix.Coords);
                        cellToFix.HasDamageHead = false;
                    }

                    cellToFix.IsDamaged = false;
                    cellToFix.HasBomb = false;
                }

                foreach (var damageHead in damageHeads) {
                    if (!cellsToFix.Contains(damageHead.CurrentCellInfo)) {
                        newDamageHeads.Add(damageHead);
                    }
                }

                damageHeads = newDamageHeads;
            }
        }
    }
}
