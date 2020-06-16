using UnityEngine;
using UnityEngine.EventSystems;

public class CellSelector : MonoBehaviour {
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

    public static void HighlightPossibleCells() {
        switch (ActionMenu.Instance.CurrentAction) {
            case ActionMenu.Action.Move:
                break;
            case ActionMenu.Action.Melee:
                break;
            case ActionMenu.Action.Range:
                break;
            case ActionMenu.Action.Bomb:
                break;
            default:
                break;
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

                    ActionMenu.Instance.UpdateMenu();
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
        ActionMenu.Instance.UpdateMenu();
        Grid.Instance.ClearSelectedCoords();
    }
}
