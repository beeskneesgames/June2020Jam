using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;

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

    private bool CheckForWin() {
        if (Turn.Instance.TurnCount > 15) {
            return true;
        }

        if (!Grid.Instance.HasDamage()) {
            return true;
        }

        return false;
    }

    private bool CheckForLoss() {
        return false;
    }

    private void TriggerWin() {
        Debug.Log($"You Win!");
    }

    private void TriggerLoss() {
        Debug.Log($"You Lose :(");
    }
}
