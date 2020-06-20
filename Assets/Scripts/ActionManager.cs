using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour {
    public const int RangedFixRange = 3;

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
                Grid.Instance.ClearActionArea();
            } else {
                currentAction = value;
            }

            if (value != Action.Melee) {
                Grid.Instance.SetActionArea(CurrentActionArea);
            }
        }
    }

    public enum Action {
        None,
        Move,
        Melee,
        Ranged,
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

    private void Start() {
        Reset();
    }

    public List<Vector2Int> CurrentActionArea {
        get {
            switch(CurrentAction) {
                case Action.Move:
                    return ActionAreaForMove();
                case Action.Melee:
                    return ActionAreaForMelee();
                case Action.Ranged:
                    return ActionAreaForRanged();
                case Action.Bomb:
                    return ActionAreaForBomb();
                default:
                    return new List<Vector2Int>();
            };
        }
    }

    public void PerformCurrentActionOn(Vector2Int coords) {
        switch (CurrentAction) {
            case Action.Move:
                Move(coords);
                break;
            case Action.Melee:
                Melee();
                break;
            case Action.Ranged:
                Ranged(coords);
                break;
            case Action.Bomb:
                Bomb(coords);
                break;
        }

        Reset();
    }

    public void Reset() {
        CurrentAction = Action.None;
        actionMenu.UnpressAllBtns();
        Grid.Instance.ClearActionArea();
    }

    private void Move(Vector2Int endCoords) {
        bool moveAllowed = true;

        foreach (var coords in Grid.PathBetween(Player.Instance.CurrentCoords, endCoords, Player.diagonalMoveAllowed)) {
            if (Grid.Instance.CellInfoAt(coords).HasObstacle) {
                moveAllowed = false;
                break;
            }
        }

        if (moveAllowed) {
            Player.Instance.MoveTo(endCoords);
        }
    }

    private void Melee() {
        Player.Instance.playerAnimator.SetTrigger("FixMelee");
        Grid.Instance.CellInfoAt(Player.Instance.CurrentCoords).MeleeFix();
    }

    private void Ranged(Vector2Int coords) {
        Player.Instance.playerAnimator.SetTrigger("FixRanged");
        Grid.Instance.CellInfoAt(coords).RangedFix();
    }

    private void Bomb(Vector2Int coords) {
        Player.Instance.playerAnimator.SetTrigger("FixRanged");
        Grid.Instance.CellInfoAt(coords).AddBomb();
    }

    private List<Vector2Int> ActionAreaForMove() {
        List<Vector2Int> unfilteredActionArea = Grid.Instance.CoordsInRadius(
            Player.Instance.CurrentCoords,
            Player.Instance.ActionPoints,
            Player.diagonalMoveAllowed
        );
        List<Vector2Int> actionArea = new List<Vector2Int>(unfilteredActionArea.Count);

        foreach (var coords in unfilteredActionArea) {
            if (!Grid.Instance.CellInfoAt(coords).HasObstacle) {
                actionArea.Add(coords);
            }
        }

        return actionArea;
    }

    private List<Vector2Int> ActionAreaForMelee() {
        List<CellInfo> unfilteredActionArea = Grid.Instance.AdjacentTo(Player.Instance.CurrentCoords, true);
        List<Vector2Int> actionArea = new List<Vector2Int>(unfilteredActionArea.Count);

        foreach (var cellInfo in unfilteredActionArea) {
            if (!cellInfo.HasObstacle) {
                actionArea.Add(cellInfo.Coords);
            }
        }

        return actionArea;
    }

    private List<Vector2Int> ActionAreaForRanged() {
        List<Vector2Int> unfilteredActionArea = Grid.Instance.CoordsInRadius(
            Player.Instance.CurrentCoords,
            RangedFixRange,
            Player.diagonalFixAllowed
        );
        List<Vector2Int> actionArea = new List<Vector2Int>(unfilteredActionArea.Count);

        foreach (var coords in unfilteredActionArea) {
            if (!Grid.Instance.CellInfoAt(coords).HasObstacle) {
                actionArea.Add(coords);
            }
        }

        return actionArea;
    }

    private List<Vector2Int> ActionAreaForBomb() {
        List<CellInfo> unfilteredActionArea = Grid.Instance.AdjacentTo(Player.Instance.CurrentCoords, true);
        List<Vector2Int> actionArea = new List<Vector2Int>(unfilteredActionArea.Count);

        foreach (var cellInfo in unfilteredActionArea) {
            if (!cellInfo.HasObstacle && !cellInfo.IsDamaged) {
                actionArea.Add(cellInfo.Coords);
            }
        }

        return actionArea;
    }
}
