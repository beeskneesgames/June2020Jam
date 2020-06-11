using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    private const float LossPercent = 0.75f;
    public WinLoseUI winLoseUI;

    public static GameManager Instance {
        get {
            return instance;
        }
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CheckEndGame() {
        if (CheckForLoss()) {
            TriggerLoss();
        } else if (CheckForWin()) {
            TriggerWin();
        }
    }

    public void Reset() {
        Debug.Log("RESET BOARD");
    }

    private bool CheckForLoss() {
        if (Grid.Instance.PercentDamaged() >= LossPercent) {
            return true;
        }

        if (Player.Instance.CurrentCell.IsDamaged) {
            return true;
        }

        return false;
    }

    private bool CheckForWin() {
        if (Turn.Instance.TurnCount >= Turn.MaxTurnCount) {
            return true;
        }

        if (!Grid.Instance.HasDamage) {
            return true;
        }

        return false;
    }

    private void TriggerWin() {
        winLoseUI.Show(true);
    }

    private void TriggerLoss() {
        winLoseUI.Show(false);
    }
}
