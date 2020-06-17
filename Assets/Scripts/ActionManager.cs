using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour {
    private static ActionManager instance;
    public ActionMenu actionMenu;

    public static ActionManager Instance {
        get {
            return instance;
        }
    }

    private Action currentAction = Action.None;
    public Action CurrentAction {
        get {
            return currentAction;
        }

        set {
            if (currentAction == value) {
                currentAction = Action.None;
                Grid.Instance.ClearActionHighlightCoords();
            } else {
                currentAction = value;
            }

            if (value != Action.Melee) {
                CellSelector.HighlightPossibleCells();
            }
        }
    }

    public enum Action {
        None,
        Move,
        Melee,
        Range,
        Bomb
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<Vector2Int> CurrentActionCoords() {
        switch (CurrentAction) {
            case Action.Move:
                return AvailableMoveCoords();
            case Action.Melee:
                return AvailableMeleeCoords();
            case Action.Range:
                return AvailableRangeCoords();
            case Action.Bomb:
                return AvailableBombCoords();
            default:
                return new List<Vector2Int>();
        }
    }

    public void PerformCurrentActionOn(Vector2Int coords) {
        actionMenu.UnpressAllBtns();

        switch (CurrentAction) {
            case Action.Move:
                Move(coords);
                break;
            case Action.Melee:
                Melee();
                break;
            case Action.Range:
                Range(coords);
                break;
            case Action.Bomb:
                Bomb(coords);
                break;
        }

        CurrentAction = Action.None;
    }

    private void Move(Vector2Int coords) {
        Player.Instance.MoveTo(coords);
        Grid.Instance.ClearActionHighlightCoords();
    }

    private void Melee() {
        Player.Instance.playerAnimator.SetTrigger("Fix");
        Grid.Instance.CellInfoAt(Player.Instance.CurrentCoords).MeleeFix();
    }

    private void Range(Vector2Int coords) {
        Player.Instance.playerAnimator.SetTrigger("Fix");
        Grid.Instance.CellInfoAt(coords).RangedFix();
        Grid.Instance.ClearActionHighlightCoords();
    }

    private void Bomb(Vector2Int coords) {
        Player.Instance.playerAnimator.SetTrigger("Fix");
        Grid.Instance.CellInfoAt(coords).AddBomb();
        Grid.Instance.ClearActionHighlightCoords();
    }

    private List<Vector2Int> AvailableMoveCoords() {
        List<Vector2Int> coords = new List<Vector2Int>();

        // TODO: Make this all cells within AP radius
        foreach (var cellInfo in Grid.Instance.AdjacentTo(Player.Instance.CurrentCoords, true)) {
            if (!cellInfo.HasObstacle && !cellInfo.IsDamaged) {
                coords.Add(cellInfo.Coords);
            }
        }

        return coords;
    }

    private List<Vector2Int> AvailableMeleeCoords() {
        List<Vector2Int> coords = new List<Vector2Int>();

        foreach (var cellInfo in Grid.Instance.AdjacentTo(Player.Instance.CurrentCoords, true)) {
            if (!cellInfo.HasObstacle && cellInfo.IsDamaged) {
                coords.Add(cellInfo.Coords);
            }
        }

        return coords;
    }

    private List<Vector2Int> AvailableRangeCoords() {
        List<Vector2Int> coords = new List<Vector2Int>();

        // TODO: Make this all cells within CellInfo.RangedFixRange radius
        foreach (var cellInfo in Grid.Instance.AdjacentTo(Player.Instance.CurrentCoords, true)) {
            if (!cellInfo.HasObstacle) {
                coords.Add(cellInfo.Coords);
            }
        }

        return coords;
    }

    private List<Vector2Int> AvailableBombCoords() {
        List<Vector2Int> coords = new List<Vector2Int>();

        foreach (var cellInfo in Grid.Instance.AdjacentTo(Player.Instance.CurrentCoords, true)) {
            if (!cellInfo.HasObstacle && !cellInfo.IsDamaged) {
                coords.Add(cellInfo.Coords);
            }
        }

        return coords;
    }
}
