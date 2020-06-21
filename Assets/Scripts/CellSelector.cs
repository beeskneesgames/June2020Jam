using UnityEngine;
using UnityEngine.EventSystems;

public class CellSelector : MonoBehaviour {
    private Camera mainCamera;
    private int cellLayerMask;
    private bool hitCellWithinActionCoords = false;

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
        hitCellWithinActionCoords = false;

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, Mathf.Infinity, cellLayerMask)) {
            hitCell = hit.transform.GetComponent<Cell>();
            foreach (var coords in ActionManager.Instance.CurrentActionArea) {
                if (coords == hitCell.Info.Coords) {
                    hitCellWithinActionCoords = true;
                }
            }
        }

        if (hitCell) {
            if (clicked) {
                if (CanPerformAction) {
                    ActionManager.Instance.PerformCurrentActionOn(hitCell.Info.Coords);
                } else {
                    ActionManager.Instance.Reset();
                }
            } else if (CanPerformAction) {
                Grid.Instance.SetHoveredCoords(hitCell.Info.Coords);
            } else {
                ClearHover();
            }
        } else {
            ClearHover();
        }
    }

    private void ClearHover() {
        Grid.Instance.ClearHoveredCoords();
    }

    private bool CanPerformAction {
        get {
            return !Player.Instance.IsPerformingAction &&
                    ActionManager.Instance.CurrentAction != ActionManager.Action.None &&
                    hitCellWithinActionCoords;
        }
    }
}
