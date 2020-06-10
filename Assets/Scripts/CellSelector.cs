using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CellSelector : MonoBehaviour {
    public TextMeshProUGUI coordsText;
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
            coordsText.text = $"({hitCell.Info.Coords.x}, {hitCell.Info.Coords.y})";

            if (clicked && CanSelect) {
                Grid.Instance.SetSelectedCoords(hitCell.Info.Coords);

                actionMenu.transform.position = new Vector3(
                    hitCell.transform.position.x + 1.5f,
                    hitCell.transform.position.y + 4.0f,
                    hitCell.transform.position.z + 3.5f
                );
                actionMenu.OpenMenu();
            } else {
                Grid.Instance.SetHoveredCoords(hitCell.Info.Coords);
            }
        } else {
            ClearHover();

            if (clicked) {
                actionMenu.CloseMenu();
                Grid.Instance.ClearSelectedCoords();
            }
        }
    }

    private void ClearHover() {
        coordsText.text = "(?, ?)";
        Grid.Instance.ClearHoveredCoords();
    }

    private bool CanSelect {
        get {
            return !Player.Instance.IsMoving;
        }
    }
}
