using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ActionMenu : MonoBehaviour {
    public GameObject panel;
    public TextMeshProUGUI moveCostText;
    public TextMeshProUGUI bombCostText;
    public Button moveBtn;
    public Button fixBtn;
    public Button bombBtn;

    public void OnCloseClicked() {
        CloseMenu();
        Grid.Instance.ClearSelectedCoords();
    }

    public void OnMoveClicked() {
        CloseMenu();
        Player.Instance.MoveTo(Grid.Instance.SelectedCoords);
        Grid.Instance.ClearSelectedCoords();
    }

    public void OnFixClicked() {
        Grid grid = Grid.Instance;
        CellInfo cell = grid.CellInfoAt(grid.SelectedCoords);

        CloseMenu();
        cell.Fix();
        Grid.Instance.ClearSelectedCoords();
    }

    public void OnBombClicked() {
        Grid grid = Grid.Instance;
        CellInfo cell = grid.CellInfoAt(grid.SelectedCoords);

        CloseMenu();
        cell.AddBomb();
        Grid.Instance.ClearSelectedCoords();
    }

    public void CloseMenu() {
        panel.SetActive(false);
    }

    public void OpenMenu() {
        panel.SetActive(true);

        if (Grid.Instance.HasSelectedCoords) {
            List<Vector2Int> path = Grid.PathBetween(Player.Instance.CurrentCell.Coords, Grid.Instance.SelectedCoords, Player.diagonalMoveAllowed);
            CellInfo selectedCell = Grid.Instance.CellInfoAt(Grid.Instance.SelectedCoords);
            bool bombInteractable = true;
            bool moveInteractable = true;
            bool fixInteractable = true;

            foreach (var coords in path) {
                CellInfo cell = Grid.Instance.CellInfoAt(coords);

                if (cell.HasObstacle) {
                    bombInteractable = false;
                    moveInteractable = false;
                    fixInteractable = false;
                }
            }

            if (Player.Instance.CurrentCoords == Grid.Instance.SelectedCoords) {
                moveInteractable = false;
                fixInteractable = false;
            }

            if (!(Player.Instance.CurrentCoords == Grid.Instance.SelectedCoords) && !Grid.Instance.AdjacentTo(Player.Instance.CurrentCoords, true).Contains(selectedCell)) {
                bombInteractable = false;
            }

            if (selectedCell.IsDamaged) {
                bombInteractable = false;
            }

            if (Player.Instance.ActionPoints < path.Count - 1) {
                bombInteractable = false;
                moveInteractable = false;
                fixInteractable = false;
            }

            bombBtn.interactable = bombInteractable;
            moveBtn.interactable = moveInteractable;
            fixBtn.interactable = fixInteractable;

            // -1 because path includes the player's square.
            moveCostText.text = $"Move/Fix cost: {path.Count - 1} AP";
            bombCostText.text = $"Bomb cost: {path.Count + 1} AP";
        } else {
            moveCostText.text = "";
            bombCostText.text = "";
        }
    }
}
