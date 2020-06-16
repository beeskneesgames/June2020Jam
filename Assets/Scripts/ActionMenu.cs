using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ActionMenu : MonoBehaviour {
    public GameObject panel;
    public Button moveBtn;
    public Button meleeBtn;
    public Button rangedBtn;
    public Button bombBtn;

    private static ActionMenu instance;

    public static ActionMenu Instance {
        get {
            return instance;
        }
    }

    private Action currentAction = Action.None;
    public Action CurrentAction {
        get {
            return currentAction;
        }

        private set {
            currentAction = value;
            if (currentAction != Action.None) {
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

    public void OnMoveClicked() {
        if (CurrentAction == Action.Move) {
            CurrentAction = Action.None;
        } else {
            CurrentAction = Action.Move;
        }

        //Player.Instance.MoveTo(Grid.Instance.SelectedCoords);
        //Grid.Instance.ClearSelectedCoords();
    }

    public void OnMeleeFixClicked() {
        if (CurrentAction == Action.Melee) {
            CurrentAction = Action.None;
        } else {
            CurrentAction = Action.Melee;
        }

        //Player.Instance.playerAnimator.SetTrigger("Fix");

        //Grid grid = Grid.Instance;
        //CellInfo cell = grid.CellInfoAt(grid.SelectedCoords);

        //cell.MeleeFix();
        //grid.ClearSelectedCoords();
    }

    public void OnRangedFixClicked() {
        if (CurrentAction == Action.Range) {
            CurrentAction = Action.None;
        } else {
            CurrentAction = Action.Range;
        }

        //Player.Instance.playerAnimator.SetTrigger("Fix");

        //Grid grid = Grid.Instance;
        //CellInfo cell = grid.CellInfoAt(grid.SelectedCoords);

        //cell.RangedFix();
        //grid.ClearSelectedCoords();
    }

    public void OnBombClicked() {
        if (CurrentAction == Action.Bomb) {
            CurrentAction = Action.None;
        } else {
            CurrentAction = Action.Bomb;
        }

        //Player.Instance.playerAnimator.SetTrigger("Fix");

        //Grid grid = Grid.Instance;
        //CellInfo cell = grid.CellInfoAt(grid.SelectedCoords);

        //cell.AddBomb();
        //grid.ClearSelectedCoords();
    }

    public List<Vector2Int> AvailableMoveCoords() {
        List<Vector2Int> coords = new List<Vector2Int>();

        // TODO: Make this all cells within AP radius
        foreach (var cellInfo in Grid.Instance.AdjacentTo(Player.Instance.CurrentCoords, true)) {
            if (!cellInfo.HasObstacle && !cellInfo.IsDamaged) {
                coords.Add(cellInfo.Coords);
            }
        }

        return coords;
    }

    public List<Vector2Int> AvailableMeleeCoords() {
        List<Vector2Int> coords = new List<Vector2Int>();

        foreach (var cellInfo in Grid.Instance.AdjacentTo(Player.Instance.CurrentCoords, true)) {
            if (!cellInfo.HasObstacle && cellInfo.IsDamaged) {
                coords.Add(cellInfo.Coords);
            }
        }

        return coords;
    }

    public List<Vector2Int> AvailableRangedCoords() {
        // TODO: Remove if cell.isDamaged is false
        // TODO: Check for path count
        //       if ((path.Count - 1) > CellInfo.RangedFixRange || (path.Count - 1) < 2) {
        List<Vector2Int> coords = new List<Vector2Int>();

        // TODO: Make this all cells within Ranged radius
        foreach (var cellInfo in Grid.Instance.AdjacentTo(Player.Instance.CurrentCoords, true)) {
            if (!cellInfo.HasObstacle && cellInfo.IsDamaged) {
                coords.Add(cellInfo.Coords);
            }
        }

        return coords;
    }

    public List<Vector2Int> AvailableBombCoords() {
        List<Vector2Int> coords = new List<Vector2Int>();

        foreach (var cellInfo in Grid.Instance.AdjacentTo(Player.Instance.CurrentCoords, true)) {
            if (!cellInfo.HasObstacle && !cellInfo.IsDamaged) {
                coords.Add(cellInfo.Coords);
            }
        }

        return coords;
    }
}
