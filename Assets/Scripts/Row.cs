using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour {
    public Cell[] cells;
    public int Index { get; set; }

    public void Resize(int newSize) {
        foreach (var cell in cells) {
            Destroy(cell);
        }

        cells = new Cell[newSize];
        float xOffset = (newSize - 1) * 0.5f;

        for (int i = 0; i < newSize; i++) {
            cells[i] = Instantiate(Grid.Instance.cellPrefab, Vector3.zero, Quaternion.identity).GetComponent<Cell>();
            cells[i].Info.Coords = new Vector2Int(Index, i);
            cells[i].transform.localPosition = new Vector3(
                xOffset - i,
                0,
                0
            );
        }
    }
}
