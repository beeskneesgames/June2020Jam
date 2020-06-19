using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ActionMenu : MonoBehaviour {
    public GameObject panel;
    public Button moveBtn;
    public Button meleeBtn;
    public Button rangedBtn;
    public Button bombBtn;

    private Color defaultColor = new Color(1.0f, 1.0f, 1.0f);
    private Color pressedColor = new Color(0.75f, 0.75f, 0.75f);

    private TextMeshProUGUI moveBtnTMP;

    private void Start() {
        string moveAPRange;
        moveBtnTMP = moveBtn.GetComponentInChildren<TextMeshProUGUI>();

        if (Player.Instance.ActionPoints == 1) {
            moveAPRange = $"{Player.Instance.ActionPoints}";
        } else {
            moveAPRange = $"1 - {Player.Instance.ActionPoints}";
        }

        moveBtnTMP.text = $"Move: {moveAPRange} AP";
        meleeBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"Melee Fix: {CellInfo.MeleeFixCost} AP";
        rangedBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"Ranged Fix: {CellInfo.RangedFixCost} AP";
        bombBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"Place Bomb: {CellInfo.BombCost} AP";
    }

    private void Update() {
        string moveAPRange;

        if (Player.Instance.ActionPoints == 1) {
            moveAPRange = $"{Player.Instance.ActionPoints}";
        } else {
            moveAPRange = $"1 - {Player.Instance.ActionPoints}";
        }

        moveBtnTMP.text = $"Move: {moveAPRange} AP";
    }

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
        ActionManager.Instance.CurrentAction = ActionManager.Action.Ranged;
    }

    public void OnBombClicked() {
        ToggleBtn(bombBtn);
        ActionManager.Instance.CurrentAction = ActionManager.Action.Bomb;
    }

    private void ToggleBtn(Button btn) {
        if (btn.image.color == defaultColor) {
            UnpressBtn(btn);
        } else {
            PressBtn(btn);
        }
    }

    private void PressBtn(Button btn) {
        btn.image.color = pressedColor;
    }

    private void UnpressBtn(Button btn) {
        btn.image.color = defaultColor;
    }

    public void UnpressAllBtns() {
        UnpressBtn(moveBtn);
        UnpressBtn(meleeBtn);
        UnpressBtn(rangedBtn);
        UnpressBtn(bombBtn);
    }
}
