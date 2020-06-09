using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenu : MonoBehaviour {
    public Grid grid;
    public GameObject panel;

    public void OnCloseClicked() {
        CloseMenu();
        grid.ClearSelectedCoords();
    }

    public void CloseMenu() {
        panel.SetActive(false);
    }

    public void OpenMenu() {
        panel.SetActive(true);
    }
}
