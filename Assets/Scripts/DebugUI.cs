using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugUI : MonoBehaviour {
    public Toggle playerDiagonalMoveToggle;
    public Toggle playerDiagonalFixToggle;
    public Toggle damageDiagonalToggle;
    public Toggle damageHealthyCellsToggle;
    public TMP_InputField gridSizeInput;

    private void Start() {
        SyncConfig();
        gridSizeInput.text = Grid.Instance.Size.x.ToString();
    }

    public void OnPlayerDiagonalMoveToggled() {
        SyncConfig();
    }

    public void OnPlayerDiagonalFixToggled() {
        SyncConfig();
    }

    public void OnDamageDiagonalToggled() {
        SyncConfig();
    }

    public void OnDamageHealthyCellsToggled() {
        SyncConfig();
    }

    public void OnGridSizeEndEdit() {
        int newGridSize;

        if (int.TryParse(gridSizeInput.text, out newGridSize)) {
            Grid.Instance.Resize(newGridSize);
            GameManager.Instance.Reset();
        } else {
            gridSizeInput.text = Grid.Instance.Size.x.ToString();
        }
    }

    private void SyncConfig() {
        Player.diagonalMoveAllowed = playerDiagonalMoveToggle.isOn;
        Player.diagonalFixAllowed = playerDiagonalFixToggle.isOn;
        DamageHead.diagonalAllowed = damageDiagonalToggle.isOn;
        DamageHead.preferHealthyCells = damageHealthyCellsToggle.isOn;
    }
}
