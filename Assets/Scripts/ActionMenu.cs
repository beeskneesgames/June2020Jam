using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

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

    public void UpdateMenu() {
        if (Grid.Instance.HasSelectedCoords) {
            List<Vector2Int> path = Grid.PathBetween(Player.Instance.CurrentCell.Coords, Grid.Instance.SelectedCoords, Player.diagonalMoveAllowed);
            CellInfo selectedCell = Grid.Instance.CellInfoAt(Grid.Instance.SelectedCoords);

            bool moveInteractable = true;
            bool meleeInteractable = true;
            bool rangedInteractable = true;
            bool bombInteractable = true;

            foreach (var coords in path) {
                CellInfo cell = Grid.Instance.CellInfoAt(coords);

                if (cell.HasObstacle) {
                    moveInteractable = false;
                    meleeInteractable = false;
                    rangedInteractable = false;
                    bombInteractable = false;
                }
            }

            if (Player.Instance.CurrentCoords == Grid.Instance.SelectedCoords) {
                moveInteractable = false;
            }

            if (!(Player.Instance.CurrentCoords == Grid.Instance.SelectedCoords) && !Grid.Instance.AdjacentTo(Player.Instance.CurrentCoords, true).Contains(selectedCell)) {
                bombInteractable = false;
                meleeInteractable = false;
            }

            List<CellInfo> damagedCoords = new List<CellInfo>();
            foreach (var cell in Grid.Instance.AdjacentTo(Player.Instance.CurrentCoords, true)) {
                if (cell.isDamaged) {
                    damagedCoords.Add(cell);
                }
            }

            if (damagedCoords.Count <= 0) {
                meleeInteractable = false;
            }

            if (selectedCell.IsDamaged) {
                bombInteractable = false;
            }

            if (!selectedCell.IsDamaged) {
                rangedInteractable = false;
            }

            if ((path.Count - 1) > CellInfo.RangedFixRange || (path.Count - 1) < 2) {
                rangedInteractable = false;
            }

            if (Player.Instance.ActionPoints < path.Count - 1) {
                moveInteractable = false;
                meleeInteractable = false;
            }

            if (Player.Instance.ActionPoints < CellInfo.BombCost) {
                bombInteractable = false;
            }

            moveBtn.interactable = moveInteractable;
            meleeBtn.interactable = meleeInteractable;
            rangedBtn.interactable = rangedInteractable;
            bombBtn.interactable = bombInteractable;

            // -1 because path includes the player's square.
            moveCostText.text = $"Move cost: {path.Count - 1} AP";
            meleeCostText.text = $"Melee cost: {CellInfo.MeleeFixCost} AP";
            rangedCostText.text = $"Ranged cost: {CellInfo.RangedFixCost} AP";
            bombCostText.text = $"Bomb cost: {CellInfo.BombCost} AP";
        } else {
            moveCostText.text = "";
            meleeCostText.text = "";
            rangedCostText.text = "";
            bombCostText.text = "";
        }
    }
}
