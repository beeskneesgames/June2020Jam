using TMPro;
using UnityEngine;

public class Turn : MonoBehaviour {
    private static Turn instance;
    private int turnCount;

    public const int MaxTurnCount = 15;
    public TextMeshProUGUI turnUI;

    public int TurnCount {
        get {
            return turnCount;
        }
        private set {
            int oldTurnCount = turnCount;
            turnCount = value;

            if (oldTurnCount < turnCount) {
                DamageManager.Instance.Spread();
                turnUI.text = $"Turn: {turnCount + 1}";
            }
            GameManager.Instance.CheckEndGame();
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
