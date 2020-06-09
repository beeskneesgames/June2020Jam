using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    private static Grid instance;
    private List<Cell> damageHeadCells = new List<Cell>();

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
            }
        }
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

    public Vector2Int ChooseDamageableCoord(Vector2Int currentCoords) {
        Vector2Int newCoord = new Vector2Int(-1, -1);

        List<int> possibleX = new List<int> {
            currentCoords.x
        };

        if (currentCoords.x > 0) {
            possibleX.Add(currentCoords.x - 1);
        }

        if (currentCoords.x < rows.Length - 1) {
            possibleX.Add(currentCoords.x + 1);
        }

        List<int> possibleY = new List<int> {
            currentCoords.y
        };

        if (currentCoords.y > 0) {
            possibleY.Add(currentCoords.y - 1);
        }

        if (currentCoords.y < rows.Length - 1) {
            possibleY.Add(currentCoords.y + 1);
        }

        List<Cell> possibleCells = new List<Cell>();

        foreach (var x in possibleX) {
            foreach (var y in possibleY) {
                Cell cell = CellAt(new Vector2Int(x, y));
                if (cell != null) {
                    possibleCells.Add(cell);
                }
            }
        }

        if (possibleCells.Count > 0) {
            newCoord = possibleCells[UnityEngine.Random.Range(0, possibleCells.Count)].Info.Coords;
        }

        return newCoord;
    }

    public void DamageCell(Vector2Int coords) {
        Cell cellToDamage = CellAt(coords);

        if (cellToDamage != null) {
            cellToDamage.Info.IsDamaged = true;
        }
    }

    public void SetDamageHeads(List<DamageHead> damageHeads) {
        foreach (var damageHeadCell in damageHeadCells) {
            damageHeadCell.Info.IsDamageHead = false;
        }

        damageHeadCells.Clear();

        foreach (var damageHead in damageHeads) {
            Cell newCell = CellAt(damageHead.Coords);
            damageHeadCells.Add(newCell);
            newCell.Info.IsDamageHead = true;
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
