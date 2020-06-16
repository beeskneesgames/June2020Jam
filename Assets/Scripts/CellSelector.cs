using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CellSelector : MonoBehaviour {
    public ActionMenu actionMenu;
    private Camera mainCamera;
    private int cellLayerMask;

    private void Start() {
        mainCamera = Camera.main;

        cellLayerMask = LayerMask.GetMask("Cells");
    }

    private void Update() {
        if (EventSystem.current.IsPointerOverGameObject()) {
            ClearHover();
        } else {
            UpdateCellMouseState();
        }
    }

    private void UpdateCellMouseState() {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Cell hitCell = null;
        bool clicked = Input.GetMouseButtonDown(0);

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, Mathf.Infinity, cellLayerMask)) {
            hitCell = hit.transform.GetComponent<Cell>();
        }

        if (hitCell) {
            if (clicked && CanSelect) {
                if (Grid.Instance.SelectedCoords == hitCell.Info.Coords) {
                    // If a cell is already selected and is clicked again,
                    // unselect it.
                    ClearSelection();
                } else {
                    Grid.Instance.SetSelectedCoords(hitCell.Info.Coords);

                    actionMenu.UpdateMenu();
                }
            } else {
                Grid.Instance.SetHoveredCoords(hitCell.Info.Coords);
            }
        } else {
            ClearHover();

            if (clicked) {
                ClearSelection();
            }
        }
    }

    private void ClearHover() {
        Grid.Instance.ClearHoveredCoords();
    }

    private bool CanSelect {
        get {
            return !Player.Instance.IsMoving;
        }
    }

    private void ClearSelection() {
        actionMenu.UpdateMenu();
        Grid.Instance.ClearSelectedCoords();
    }
}
