﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public Row[] rows;

    private Vector2Int hoveredCoords = new Vector2Int(-1, -1);
    public bool HasHoveredCoords {
        get {
            return hoveredCoords.x >= 0;
        }
    }

    private void Start() {
        for (int i = 0; i < rows.Length; i++) {
            Row row = rows[i]; // your boat
            row.Index = i;

            for (int j = 0; j < row.cells.Length; j++) {
                Cell cell = row.cells[j];
                cell.Row = i;
                cell.Col = j;
            }
        }
    }

    public void SetHoveredCoords(int row, int col) {
        if (HasHoveredCoords) {
            HoveredCell.UnHighlight();
        }

        hoveredCoords = new Vector2Int(row, col);
        HoveredCell.Highlight();
    }

    public void ClearHoveredCoords() {
        if (HasHoveredCoords) {
            HoveredCell.UnHighlight();
        }

        hoveredCoords = new Vector2Int(-1, -1);
    }

    private Cell CellAt(int row, int col) {
        return rows[row].cells[col];
    }

    private Cell HoveredCell {
        get {
            if (HasHoveredCoords) {
                return CellAt(hoveredCoords.x, hoveredCoords.y);
            } else {
                return null;
            }
        }
    }
}
