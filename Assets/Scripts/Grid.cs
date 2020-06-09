using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    private static Grid instance;
    private List<Cell> damageHeadCells = new List<Cell>();
    private List<CellInfo> allCells = new List<CellInfo>();
    private int cellCount;

    public static Grid Instance {
        get {
            return instance;
        }
    }

    public Row[] rows;

    private Vector2Int hoveredCoords = new Vector2Int(-1, -1);
    public bool HasHoveredCoords {
        get {
            return hoveredCoords.x >= 0;
        }
    }

    private Vector2Int selectedCoords = new Vector2Int(-1, -1);
    public Vector2Int SelectedCoords {
        get {
            return selectedCoords;
        }
    }
    public bool HasSelectedCoords {
        get {
            return SelectedCoords.x >= 0;
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
        for (int i = 0; i < rows.Length; i++) {
            Row row = rows[i]; // your boat
            row.Index = i;

            for (int j = 0; j < row.cells.Length; j++) {
                Cell cell = row.cells[j];
                cell.Info.Coords = new Vector2Int(i, j);
                cellCount++;

                allCells.Add(cell.Info);
            }
        }
    }

    public bool HasDamage() {
        foreach (var cell in allCells) {
            if (cell.IsDamaged) {
                return true;
            }
        }

        return false;
    }

    public float PercentageDamaged() {
        List<CellInfo> cellsWithDamage = new List<CellInfo>();

        foreach (var cell in allCells) {
            if (cell.IsDamaged) {
                cellsWithDamage.Add(cell);
            }
        }

        return ((float)cellsWithDamage.Count / (float)cellCount);
    }

    public void SetHoveredCoords(Vector2Int coords) {
        if (HasHoveredCoords && HoveredCell != SelectedCell) {
            HoveredCell.CurrentMouseState = Cell.MouseState.None;
        }

        hoveredCoords = coords;

        if (HoveredCell != SelectedCell) {
            // If a cell is both hovered and selected, we just want to show the
            // selected color.
            HoveredCell.CurrentMouseState = Cell.MouseState.Hovered;
        }
    }

    public void ClearHoveredCoords() {
        if (HasHoveredCoords) {
            HoveredCell.CurrentMouseState = Cell.MouseState.None;
        }

        hoveredCoords = new Vector2Int(-1, -1);
    }

    public void SetSelectedCoords(Vector2Int coords) {
        if (HasSelectedCoords) {
            SelectedCell.CurrentMouseState = Cell.MouseState.None;
        }

        selectedCoords = coords;
        SelectedCell.CurrentMouseState = Cell.MouseState.Selected;
    }

    public void ClearSelectedCoords() {
        if (HasSelectedCoords) {
            SelectedCell.CurrentMouseState = Cell.MouseState.None;
        }

        selectedCoords = new Vector2Int(-1, -1);
    }

    public CellInfo[] AdjacentTo(Vector2Int coords) {
        List<int> possibleX = new List<int> {
            coords.x
        };

        if (coords.x > 0) {
            possibleX.Add(coords.x - 1);
        }

        if (coords.x < rows.Length - 1) {
            possibleX.Add(coords.x + 1);
        }

        List<int> possibleY = new List<int> {
            coords.y
        };

        if (coords.y > 0) {
            possibleY.Add(coords.y - 1);
        }

        if (coords.y < rows.Length - 1) {
            possibleY.Add(coords.y + 1);
        }

        List<CellInfo> cellInfos = new List<CellInfo>();

        foreach (var x in possibleX) {
            foreach (var y in possibleY) {
                Cell cell = CellAt(new Vector2Int(x, y));
                if (cell != null) {
                    cellInfos.Add(cell.Info);
                }
            }
        }

        return cellInfos.ToArray();
    }

    public void SetDamageHeads(List<DamageHead> damageHeads) {
        foreach (var damageHeadCell in damageHeadCells) {
            damageHeadCell.Info.HasDamageHead = false;
        }

        damageHeadCells.Clear();

        foreach (var damageHead in damageHeads) {
            Cell newCell = CellAt(damageHead.Coords);
            damageHeadCells.Add(newCell);
            newCell.Info.HasDamageHead = true;
        }
    }

    public CellInfo CellInfoAt(Vector2Int coords) {
        return CellAt(coords)?.Info;
    }

    private Cell CellAt(Vector2Int coords) {
        if (coords.x < 0 ||
            coords.x > rows.Length ||
            coords.y < 0 ||
            coords.y > rows.Length) {
            return null;
        }

        return rows[coords.x].cells[coords.y];
    }

    public Vector3 PositionForCoords(Vector2Int coords) {
        return CellAt(coords).transform.position;
    }

    private Cell HoveredCell {
        get {
            if (HasHoveredCoords) {
                return CellAt(hoveredCoords);
            } else {
                return null;
            }
        }
    }

    private Cell SelectedCell {
        get {
            if (HasSelectedCoords) {
                return CellAt(SelectedCoords);
            } else {
                return null;
            }
        }
    }
}
