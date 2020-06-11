using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    private const float LossPercent = 0.75f;

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
        Debug.Log("You Win!");
    }

    private void TriggerLoss() {
        Debug.Log("You Lose :(");
    }
}
