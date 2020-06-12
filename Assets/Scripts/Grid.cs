using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public GameObject cellPrefab;
    public GameObject rowPrefab;
    public Vector2Int Size = new Vector2Int(12, 12);

    private static Grid instance;
    private List<Cell> damageHeadCells = new List<Cell>();
    private List<CellInfo> allCells;
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
        Reset();
    }

    public void Reset() {
        damageHeadCells = new List<Cell>();
        allCells = new List<CellInfo>();

        for (int i = 0; i < rows.Length; i++) {
            Row row = rows[i]; // your boat
            row.Index = i;

            for (int j = 0; j < row.cells.Length; j++) {
                Cell cell = row.cells[j];
                cell.Info.Coords = new Vector2Int(i, j);
                cellCount++;

                cell.Info.IsDamaged = false;

                if (cell.Info.HasDamageHead) {
                    DamageManager.Instance.RemoveHeadsAt(cell.Info.Coords);
                    cell.Info.HasDamageHead = false;
                }

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

    public CellInfo[] AdjacentTo(Vector2Int coords, bool includeDiagonal) {
        // Add all the fully adjacent cells.
        List<Cell> cells = new List<Cell> {
            CellAt(coords + new Vector2Int( 1,  0)),
            CellAt(coords + new Vector2Int(-1,  0)),
            CellAt(coords + new Vector2Int( 0,  1)),
            CellAt(coords + new Vector2Int( 0, -1))
        };

        // Add all the diagonally adjacent cells if requested.
        if (includeDiagonal) {
            cells.Add(CellAt(coords + new Vector2Int(1, 1)));
            cells.Add(CellAt(coords + new Vector2Int(-1, 1)));
            cells.Add(CellAt(coords + new Vector2Int(1, -1)));
            cells.Add(CellAt(coords + new Vector2Int(-1, -1)));
        }

        List<CellInfo> cellInfos = new List<CellInfo>();

        // Convert Cells to CellInfos and trim out nulls.
        foreach (var cell in cells) {
            if (cell != null) {
                cellInfos.Add(cell.Info);
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

    public static List<Vector2Int> PathBetween(Vector2Int start, Vector2Int end, bool diagonalAllowed) {
        if (diagonalAllowed) {
            return PathBetweenWithDiagonals(start, end);
        } else {
            return PathBetweenWithoutDiagonals(start, end);
        }
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

    public CellInfo RetrieveRandomCell(int buffer = 0) {
        int randomX = Random.Range(0, Size.x - buffer);
        int randomY = Random.Range(0, Size.y - buffer);

        return CellInfoAt(new Vector2Int(randomX, randomY));
    }

    public void Resize(int newSize) {
        foreach (var row in rows) {
            foreach (var cell in row.cells) {
                Destroy(cell);
            }

            Destroy(row);
        }

        Size = new Vector2Int(newSize, newSize);
        rows = new Row[Size.x];
        float zOffset = (Size.x - 1) * -0.5f;

        for (int i = 0; i < Size.x; i++) {
            rows[i] = Instantiate(rowPrefab, Vector3.zero, Quaternion.identity).GetComponent<Row>();
            rows[i].Resize(Size.y);
            rows[i].Index = i;
            rows[i].transform.localPosition = new Vector3(
                0,
                0,
                zOffset + i
            );
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
                ShowPath(PathBetween(Player.Instance.CurrentCell.Coords, endCoords, Player.diagonalMoveAllowed));
            } else {
                ClearPath();
            }
        }

    }

    private Cell CellAt(Vector2Int coords) {
        if (!InBounds(coords)) {
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

    private bool InBounds(Vector2Int coords) {
        return
            coords.x >= 0 &&
            coords.x < Size.x &&
            coords.y >= 0 &&
            coords.y < Size.y;
    }

    private static List<Vector2Int> PathBetweenWithDiagonals(Vector2Int start, Vector2Int end) {
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

        // Now that we've finished diagonal traversal, traverse non-diagonally
        // the rest of the way.
        List<Vector2Int> remainingPath = PathBetweenWithoutDiagonals(current, end);
        remainingPath.RemoveAt(0); // Remove the first coords, since we already added them.
        coords.AddRange(remainingPath);

        return coords;
    }

    private static List<Vector2Int> PathBetweenWithoutDiagonals(Vector2Int start, Vector2Int end) {
        List<Vector2Int> coords = new List<Vector2Int> { start };
        Vector2Int current = start;

        // First, move along the X axis until we're on the same X coord as the
        // end cell.
        while (current.x != end.x) {
            current += new Vector2Int(current.x < end.x ? 1 : -1, 0);
            coords.Add(current);
        }

        // Then, move along the Y axis until we're on the same Y coord as the
        // end cell.
        while (current.y != end.y) {
            current += new Vector2Int(0, current.y < end.y ? 1 : -1);
            coords.Add(current);
        }

        return coords;
    }
}
