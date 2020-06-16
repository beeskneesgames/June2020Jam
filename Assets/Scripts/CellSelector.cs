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
        switch (ActionManager.Instance.CurrentAction) {
            case ActionManager.Action.Move:
                Grid.Instance.SetActionHighlightCoords(ActionManager.Instance.AvailableMoveCoords());
                break;
            case ActionManager.Action.Melee:
                Grid.Instance.SetActionHighlightCoords(ActionManager.Instance.AvailableMeleeCoords());
                break;
            case ActionManager.Action.Range:
                Grid.Instance.SetActionHighlightCoords(ActionManager.Instance.AvailableRangeCoords());
                break;
            case ActionManager.Action.Bomb:
                Grid.Instance.SetActionHighlightCoords(ActionManager.Instance.AvailableBombCoords());
                break;
            case ActionManager.Action.None:
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

        if (hitCell && ActionManager.Instance.CurrentAction != ActionManager.Action.None) {
            if (clicked && CanSelect) {
                Debug.Log("Do action");
                // TODO: Check if we are within bounds
                //       If so, do action
            } else {
                Grid.Instance.SetHoveredCoords(hitCell.Info.Coords);
            }
        } else {
            ClearHover();
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
}
