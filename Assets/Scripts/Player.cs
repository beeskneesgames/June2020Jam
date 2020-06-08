using UnityEngine;

public class Player : MonoBehaviour {
    private int actionPoints;
    private int maxPoints = 5;

    public int ActionPoints {
        get {
            return actionPoints;
        }
        set {
            if (actionPoints <= 0) {
                Turn.Instance.EndTurn();
            }
        }
    }
}
