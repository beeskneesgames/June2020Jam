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
        ActionManager.Instance.CurrentAction = ActionManager.Action.Move;
    }

    public void OnMeleeFixClicked() {
        ActionManager.Instance.CurrentAction = ActionManager.Action.Melee;
    }

    public void OnRangedFixClicked() {
        ActionManager.Instance.CurrentAction = ActionManager.Action.Range;
    }

    public void OnBombClicked() {
        ActionManager.Instance.CurrentAction = ActionManager.Action.Bomb;
    }
}
