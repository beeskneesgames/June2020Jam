using UnityEngine;

public class Turn : MonoBehaviour {
    private static Turn instance;
    public int TurnCount { get; private set; }

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
    }

    public static void EndTurn() {
        Turn.Instance.TurnCount++;
    }
}
