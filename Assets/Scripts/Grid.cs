using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    private static Grid instance;

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
                cell.Coords = new Vector2Int(i, j);
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
        Vector2Int newCoord;

        List<int> possibleRows = new List<int> {
            currentCoords.x - 1,
            currentCoords.x,
            currentCoords.x + 1
        };

        List<int> possibleCols = new List<int> {
            currentCoords.y - 1,
            currentCoords.y,
            currentCoords.y + 1
        };

        do {
            newCoord = new Vector2Int(
                possibleRows[UnityEngine.Random.Range(0, possibleRows.Count)],
                possibleCols[UnityEngine.Random.Range(0, possibleCols.Count)]
            );
        } while (
            CellAt(newCoord) == null ||
            CellAt(newCoord).IsDamaged ||
            possibleRows.Count == 0 ||
            possibleCols.Count == 0
        );

        return newCoord;
    }

    public void DamageCell(Vector2Int coords) {
        Cell cellToDamage = CellAt(coords);
        cellToDamage.IsDamaged = true;
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
