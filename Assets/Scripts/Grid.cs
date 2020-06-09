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

    public Vector2Int[] FindDamageableCoords(Vector2Int currentCoords) {
        //TODO: Find damageable adjacent coordinates
        List<Vector2Int> coords = new List<Vector2Int> { currentCoords };

        return coords.ToArray();
    }

    public void DamageCell(Vector2Int coords) {
        Cell cellToDamage = CellAt(coords);
        cellToDamage.IsDamaged = true;
    }

    private Cell CellAt(Vector2Int coords) {
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
