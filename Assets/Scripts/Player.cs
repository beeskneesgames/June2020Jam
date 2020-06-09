using UnityEngine;

public class Player : MonoBehaviour {
    private static Player instance;

    public static Player Instance {
        get {
            return instance;
        }
    }

    private const int MaxPoints = 5;
    private int actionPoints;

    public int ActionPoints {
        get {
            return actionPoints;
        }
        set {
            actionPoints = value;

            if (actionPoints <= 0) {
                EndTurn();
            }
        }
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void MoveTo(Vector2Int coords) {
        //TODO Move

        GameManager.Instance.CheckEndGame();
    }

    private void EndTurn() {
        Turn.Instance.EndTurn();
        ActionPoints = MaxPoints;
    }
}
