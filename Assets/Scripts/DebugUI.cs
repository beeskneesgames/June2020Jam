using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour {
    public Toggle playerDiagonalMoveToggle;
    public Toggle playerDiagonalFixToggle;
    public Toggle damageDiagonalToggle;
    public Toggle damageHealthyCellsToggle;

    private void Start() {
        SyncConfig();
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

    private void SyncConfig() {
        Player.diagonalMoveAllowed = playerDiagonalMoveToggle.isOn;
        Player.diagonalFixAllowed = playerDiagonalFixToggle.isOn;
        DamageHead.diagonalAllowed= damageDiagonalToggle.isOn;
        DamageHead.preferHealthyCells = damageHealthyCellsToggle.isOn;
    }
}
