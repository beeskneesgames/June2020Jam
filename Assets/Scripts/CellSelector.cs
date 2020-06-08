using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellSelector : MonoBehaviour {
    public TextMeshProUGUI coordsText;
    public Grid grid;
    private Camera mainCamera;
    private int cellLayerMask;

    private void Start() {
        mainCamera = Camera.main;
        cellLayerMask = LayerMask.GetMask("Cells");
    }

    private void Update() {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Cell hitCell = null;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, cellLayerMask)) {
            hitCell = hit.transform.GetComponent<Cell>();
        }

        if (hitCell) {
            coordsText.text = $"({hitCell.Coords.x}, {hitCell.Coords.y})";
            grid.SetHoveredCoords(hitCell.Coords);
        } else {
            coordsText.text = "(?, ?)";
            grid.ClearHoveredCoords();
        }

    }
}
