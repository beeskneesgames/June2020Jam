﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CellSelector : MonoBehaviour {
    public TextMeshProUGUI coordsText;
    public Grid grid;
    public Canvas actionCanvas;
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
        bool clicked = Input.GetMouseButtonDown(0);

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, Mathf.Infinity, cellLayerMask)) {
            hitCell = hit.transform.GetComponent<Cell>();
        }

        if (hitCell) {
            coordsText.text = $"({hitCell.Coords.x}, {hitCell.Coords.y})";

            if (clicked) {
                actionCanvas.transform.position = new Vector3(
                    hitCell.transform.position.x + 2.0f,
                    hitCell.transform.position.y + 4.0f,
                    hitCell.transform.position.z + 3.5f
                );
                // TODO do this nicer
                actionCanvas.transform.GetChild(0).gameObject.SetActive(true);
                grid.SetSelectedCoords(hitCell.Coords);
            } else {
                grid.SetHoveredCoords(hitCell.Coords);
            }
        } else {
            coordsText.text = "(?, ?)";
            grid.ClearHoveredCoords();

            if (clicked) {
                // TODO do this nicer
                actionCanvas.transform.GetChild(0).gameObject.SetActive(false);
                grid.ClearSelectedCoords();
            }
        }

    }
}
