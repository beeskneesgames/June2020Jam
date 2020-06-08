using UnityEngine;

public class Player : MonoBehaviour {
    private const int MaxPoints = 5;
    private int actionPoints;

    public int ActionPoints {
        get {
            return actionPoints;
        }
        set {
            if (actionPoints <= 0) {
                EndTurn();
            }
        }
    }

    private void EndTurn() {
        Turn.Instance.EndTurn();
        actionPoints = MaxPoints;
    }
}
