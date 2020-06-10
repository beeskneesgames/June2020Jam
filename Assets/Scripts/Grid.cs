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

    public Vector2Int SelectedCoords { get; private set; } = new Vector2Int(-1, -1);
    public bool HasSelectedCoords {
        get {
            return SelectedCoords.x >= 0;
        }
    }

    public bool HasDamage {
        get {
            foreach (var cell in allCells) {
                if (cell.IsDamaged) {
                    return true;
                }
            }

            return false;
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

    public float PercentDamaged() {
        List<CellInfo> cellsWithDamage = new List<CellInfo>();

        foreach (var cell in allCells) {
            if (cell.IsDamaged) {
                cellsWithDamage.Add(cell);
            }
        }

        return cellsWithDamage.Count / (float)cellCount;
    }

    public void SetHoveredCoords(Vector2Int coords) {
        if (coords == hoveredCoords) {
            // Skip if the hovered coords didn't change.
            return;
        }

        if (HasHoveredCoords && HoveredCell != SelectedCell) {
            HoveredCell.CurrentMouseState = Cell.MouseState.None;
        }

        hoveredCoords = coords;

        if (HoveredCell != SelectedCell) {
            // If a cell is both hovered and selected, we just want to show the
            // selected color.
            HoveredCell.CurrentMouseState = Cell.MouseState.Hovered;
        }

        UpdateDisplayedPath();
    }

    public void ClearHoveredCoords() {
        if (HasHoveredCoords && HoveredCell != SelectedCell) {
            HoveredCell.CurrentMouseState = Cell.MouseState.None;
        }

        hoveredCoords = new Vector2Int(-1, -1);

        UpdateDisplayedPath();
    }

    public void SetSelectedCoords(Vector2Int coords) {
        if (HasSelectedCoords) {
            SelectedCell.CurrentMouseState = Cell.MouseState.None;
        }

        SelectedCoords = coords;
        SelectedCell.CurrentMouseState = Cell.MouseState.Selected;

        UpdateDisplayedPath();
    }

    public void ClearSelectedCoords() {
        if (HasSelectedCoords) {
            SelectedCell.CurrentMouseState = Cell.MouseState.None;
        }

        SelectedCoords = new Vector2Int(-1, -1);

        UpdateDisplayedPath();
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

    public static List<Vector2Int> PathBetween(Vector2Int start, Vector2Int end) {
        List<Vector2Int> coords = new List<Vector2Int> { start };
        Vector2Int current = start;
        int xOffset;
        int yOffset;

        if (current.x < end.x) {
            // The end cell is *above* the start cell on the X axis. This means
            // we'll want to start out by moving *up* the X axis.
            xOffset = 1;
        } else if (current.x > end.x) {
            // The end cell is *below* the start cell on the X axis. This means
            // we'll want to start out by moving *down* the X axis.
            xOffset = -1;
        } else {
            // The end cell is on the *same* X coord as the start cell.
            // This means we'll always want to *stay still* on the X axis.
            xOffset = 0;
        }

        if (current.y < end.y) {
            // The end cell is *above* the start cell on the Y axis. This means
            // we'll want to start out by moving *up* the Y axis.
            yOffset = 1;
        } else if (current.y > end.y) {
            // The end cell is *below* the start cell on the Y axis. This means
            // we'll want to start out by moving *down* the Y axis.
            yOffset = -1;
        } else {
            // The end cell is on the *same* Y coord as the start cell.
            // This means we'll always want to *stay still* on the Y axis.
            yOffset = 0;
        }

        // First, if necessary, move diagonally until we're on either the same
        // X or Y coord as the end cell.
        while (current.x != end.x && current.y != end.y) {
            current += new Vector2Int(xOffset, yOffset);
            coords.Add(current);
        }

        // If we're on the same X coord as the end cell, stop moving on it and
        // just move on the Y axis until we're at the end cell
        if (current.x == end.x) {
            xOffset = 0;
        }

        // If we're on the same Y coord as the end cell, stop moving on it and
        // just move on the X axis until we're at the end cell
        if (current.y == end.y) {
            yOffset = 0;
        }

        // Now that we've updated the offsets to stop moving us diagonally,
        // keep moving with them until we reach the end cell.
        while (current != end) {
            current += new Vector2Int(xOffset, yOffset);
            coords.Add(current);
        }

        return coords;
    }

    public void ShowPath(List<Vector2Int> path) {
        ClearPath();

        foreach (Vector2Int coords in path) {
            CellAt(coords).inPath = true;
        }
    }

    public void ClearPath() {
        foreach (Row row in rows) {
            foreach (Cell cell in row.cells) {
                cell.inPath = false;
            }
        }
    }

    public CellInfo CellInfoAt(Vector2Int coords) {
        Cell cell = CellAt(coords);

        if (cell == null) {
            return null;
        } else {
            return cell.Info;
        }
    }

    private void UpdateDisplayedPath() {

        if (Player.Instance.IsMoving) {
            ShowPath(Player.Instance.MovementPath);
        } else {
            Vector2Int endCoords;

            if (HasSelectedCoords) {
                endCoords = SelectedCoords;
            } else if (HasHoveredCoords) {
                endCoords = hoveredCoords;
            } else {
                endCoords = new Vector2Int(-1, -1);
            }

            if (endCoords.x >= 0) {
                ShowPath(PathBetween(Player.Instance.CurrentCell.Coords, endCoords));
            } else {
                ClearPath();
            }
        }

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
