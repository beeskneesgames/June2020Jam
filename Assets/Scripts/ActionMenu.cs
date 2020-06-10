using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionMenu : MonoBehaviour {
    public GameObject panel;
    public TextMeshProUGUI costText;

    private List<Vector2Int> currentPath = null;

    public void OnCloseClicked() {
        CloseMenu();
        Grid.Instance.ClearSelectedCoords();
    }

    public void OnMoveClicked() {
        CloseMenu();
        Player.Instance.MoveTo(Grid.Instance.SelectedCoords);
        Grid.Instance.ClearSelectedCoords();
    }

    public void OnMovePointerEntered() {
        Grid.Instance.ShowPath(currentPath);
    }

    public void OnMovePointerExited() {
        Grid.Instance.ClearPath();
    }

    public void OnFixClicked() {
        Grid grid = Grid.Instance;
        CellInfo cell = grid.CellInfoAt(grid.SelectedCoords);

        CloseMenu();
        cell.Fix();
        Grid.Instance.ClearSelectedCoords();
    }

    public void CloseMenu() {
        currentPath = null;
        Grid.Instance.ClearPath();
        panel.SetActive(false);
    }

    public void OpenMenu() {
        panel.SetActive(true);

        if (Grid.Instance.HasSelectedCoords) {
            currentPath = Grid.PathBetween(Player.Instance.CurrentCell.Coords, Grid.Instance.SelectedCoords);

            // -1 because path includes the player's square.
            costText.text = $"Cost: {currentPath.Count - 1} AP";
        } else {
            costText.text = "";
        }
    }
}
