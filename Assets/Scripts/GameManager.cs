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
        Debug.Log($"Check if we should end the game");

        if (CheckForWin()) {
            TriggerWin();
        } else if (CheckForLoss()) {
            TriggerLoss();
        }
    }

    private bool CheckForWin() {
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
