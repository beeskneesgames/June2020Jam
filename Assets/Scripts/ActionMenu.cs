﻿using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ActionMenu : MonoBehaviour {
    public GameObject panel;
    public TextMeshProUGUI costText;
    public Button moveBtn;
    public Button fixBtn;

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

    public void CloseMenu() {
        panel.SetActive(false);
    }

    public void OpenMenu() {
        panel.SetActive(true);

        if (Grid.Instance.HasSelectedCoords) {
            List<Vector2Int> path = Grid.PathBetween(Player.Instance.CurrentCell.Coords, Grid.Instance.SelectedCoords);

            if (path.Count <= Player.Instance.ActionPoints) {
                moveBtn.interactable = true;
                fixBtn.interactable = true;
            } else {
                moveBtn.interactable = false;
                fixBtn.interactable = false;
            }

            // -1 because path includes the player's square.
            costText.text = $"Cost: {path.Count - 1} AP";
        } else {
            costText.text = "";
        }
    }
}
