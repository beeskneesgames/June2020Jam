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
                Grid.Instance.SetActionHighlightCoords(ActionMenu.Instance.AvailableMoveCoords());
                break;
            case ActionMenu.Action.Melee:
                Grid.Instance.SetActionHighlightCoords(ActionMenu.Instance.AvailableMeleeCoords());
                break;
            case ActionMenu.Action.Range:
                Grid.Instance.SetActionHighlightCoords(ActionMenu.Instance.AvailableRangeCoords());
                break;
            case ActionMenu.Action.Bomb:
                Grid.Instance.SetActionHighlightCoords(ActionMenu.Instance.AvailableBombCoords());
                break;
            case ActionMenu.Action.None:
                Grid.Instance.ClearActionHighlightCoords();
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
                Debug.Log("Do action");
                // TODO: Check if we are within bounds
                //       If so, do action

                //if (Grid.Instance.SelectedCoords == hitCell.Info.Coords) {
                    // If a cell is already selected and is clicked again,
                    // unselect it.
                    //ClearSelection();
                //} else {
                    //Grid.Instance.SetSelectedCoords(hitCell.Info.Coords);
                //}
            } else {
                Grid.Instance.SetHoveredCoords(hitCell.Info.Coords);
            }
        } else {
            ClearHover();

            if (clicked) {
                //ClearSelection();
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
        //Grid.Instance.ClearSelectedCoords();
    }
}
