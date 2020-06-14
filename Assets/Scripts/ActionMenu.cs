﻿using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ActionMenu : MonoBehaviour {
    public GameObject panel;
    public TextMeshProUGUI moveCostText;
    public TextMeshProUGUI meleeCostText;
    public TextMeshProUGUI rangedCostText;
    public TextMeshProUGUI bombCostText;
    public Button moveBtn;
    public Button meleeBtn;
    public Button rangedBtn;
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

    public void OnMeleeFixClicked() {
        Grid grid = Grid.Instance;
        CellInfo cell = grid.CellInfoAt(grid.SelectedCoords);

        CloseMenu();
        cell.MeleeFix();
        Grid.Instance.ClearSelectedCoords();
    }

    public void OnRangedFixClicked() {
        Grid grid = Grid.Instance;
        CellInfo cell = grid.CellInfoAt(grid.SelectedCoords);

        CloseMenu();
        cell.MeleeFix();
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
