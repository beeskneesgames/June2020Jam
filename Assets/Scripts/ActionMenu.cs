using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenu : MonoBehaviour {
    public GameObject panel;

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
        Grid.Instance.ShowPath(
            Grid.PathBetween(Player.Instance.CurrentCell.Coords, Grid.Instance.SelectedCoords)
        );
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
        Grid.Instance.ClearPath();
        panel.SetActive(false);
    }

    public void OpenMenu() {
        panel.SetActive(true);
    }
}
