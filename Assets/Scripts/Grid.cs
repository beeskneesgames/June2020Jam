using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public Row[] rows;

    private void Start() {
        for (int i = 0; i < rows.Length; i++) {
            for (int j = 0; j < rows[i].cells.Length; j++) {
                Cell cell = rows[i].cells[j];
                cell.Row = i;
                cell.Col = j;
            }
        }
    }
}
