using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenu : MonoBehaviour {
    public GameObject panel;

    public void OnCloseClicked() {
        Debug.Log("In ActionMenu.OnCloseClicked");
        CloseMenu();
        Grid.Instance.ClearSelectedCoords();
    }

    public void OnMoveClicked() {
        Debug.Log("In ActionMenu.OnMoveClicked");
        CloseMenu();
        Player.Instance.MoveTo(Grid.Instance.SelectedCoords);
        Grid.Instance.ClearSelectedCoords();
    }

    public void CloseMenu() {
        panel.SetActive(false);
    }

    public void OpenMenu() {
        panel.SetActive(true);
    }
}
