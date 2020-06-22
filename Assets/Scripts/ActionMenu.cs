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
            moveAPRange = $"1-{Player.Instance.ActionPoints}";
        }

        moveBtnTMP.text = $"{moveAPRange}";
        meleeBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"{CellInfo.MeleeFixCost}";
        rangedBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"{CellInfo.RangedFixCost}";
        bombBtn.GetComponentInChildren<TextMeshProUGUI>().text = $"{CellInfo.BombCost}";
    }

    private void Update() {
        string moveAPRange;

        if (Player.Instance.ActionPoints == 1) {
            moveAPRange = $"{Player.Instance.ActionPoints}";
        } else {
            moveAPRange = $"1-{Player.Instance.ActionPoints}";
        }

        moveBtnTMP.text = $"{moveAPRange}";
    }

    public void OnMoveClicked() {
        AudioManager.Instance.PlayBtnClick();
        ActionManager.Instance.CurrentAction = ActionManager.Action.Move;
    }

    public void OnMeleeFixClicked() {
        AudioManager.Instance.PlayBtnClick();
        ActionManager.Instance.CurrentAction = ActionManager.Action.Melee;
        ActionManager.Instance.PerformCurrentActionOn(Player.Instance.CurrentCoords);
    }

    public void OnRangedFixClicked() {
        AudioManager.Instance.PlayBtnClick();
        ActionManager.Instance.CurrentAction = ActionManager.Action.Ranged;
    }

    public void OnBombClicked() {
        AudioManager.Instance.PlayBtnClick();
        ActionManager.Instance.CurrentAction = ActionManager.Action.Bomb;
    }
}
