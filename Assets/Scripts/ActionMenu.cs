using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ActionMenu : MonoBehaviour {
    public GameObject panel;
    public Button moveBtn;
    public Button meleeBtn;
    public Button rangedBtn;
    public Button bombBtn;

    public void OnMoveClicked() {
        ToggleBtn(moveBtn);
        ActionManager.Instance.CurrentAction = ActionManager.Action.Move;
    }

    public void OnMeleeFixClicked() {
        ActionManager.Instance.CurrentAction = ActionManager.Action.Melee;
        ActionManager.Instance.PerformCurrentActionOn(Player.Instance.CurrentCoords);
    }

    public void OnRangedFixClicked() {
        ToggleBtn(rangedBtn);
        ActionManager.Instance.CurrentAction = ActionManager.Action.Range;
    }

    public void OnBombClicked() {
        ToggleBtn(bombBtn);
        ActionManager.Instance.CurrentAction = ActionManager.Action.Bomb;
    }

    private void ToggleBtn(Button btn) {
        Color defaultColor = new Color(1.0f, 1.0f, 1.0f);
        Color pressedColor = new Color(0.75f, 0.75f, 0.75f);

        if (btn.image.color == defaultColor) {
            btn.image.color = pressedColor;
        } else {
            btn.image.color = defaultColor;
        }
    }
}
