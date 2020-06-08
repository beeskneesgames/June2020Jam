using UnityEngine;

public class Player : MonoBehaviour {
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

    public void MoveTo(Vector2Int coords) {
        //TODO Move

        GameManager.Instance.CheckEndGame();
    }

    private void EndTurn() {
        Turn.Instance.EndTurn();
        ActionPoints = MaxPoints;
    }
}
