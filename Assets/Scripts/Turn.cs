using UnityEngine;

public class Turn : MonoBehaviour {
    private static Turn instance;
    private int turnCount;
    private const int MaxTurnCount = 15;

    public int TurnCount {
        get {
            return turnCount;
        }
        private set {
            turnCount = value;
            GameManager.Instance.CheckEndGame();
            DamageManager.Instance.Spread();
        }
    }

    public static Turn Instance {
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
        Debug.Log($"Turn Count: {Turn.Instance.TurnCount}");
    }

    public void EndTurn() {
        Turn.Instance.TurnCount++;
        Debug.Log($"Turn Count: {Turn.Instance.TurnCount}");
    }
}
