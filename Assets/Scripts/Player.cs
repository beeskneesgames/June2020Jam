using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour {
    public static bool diagonalFixAllowed = false;
    public static bool diagonalMoveAllowed = false;

    private static Player instance;
    public static Player Instance {
        get {
            return instance;
        }
    }

    // Movement
    public bool IsMoving { get; private set; } = false;
    private System.Action moveCallback;
    private List<Vector2Int> remainingMovementPath = null;
    public List<Vector2Int> MovementPath { get; private set; }
    private Vector2Int targetCoords = new Vector2Int(-1, -1);
    private Vector3 startPositionForMove;
    private Vector3 endPositionForMove;
    private float timeMoving;
    private const float MaxTimeMoving = 0.25f;

    public Vector2Int CurrentCoords { get; private set; }

    public CellInfo CurrentCell {
        get {
            return Grid.Instance.CellInfoAt(CurrentCoords);
        }
    }

    // Action Points
    public TextMeshProUGUI actionPointUI;
    private const int MaxPoints = 2;
    private int actionPoints;
    public int ActionPoints {
        get {
            return actionPoints;
        }

        set {
            actionPoints = value;
            actionPointUI.text = $"Action Points: {actionPoints}";

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
        ActionPoints = MaxPoints;
    }

    private void Start() {
        Reset();
    }

    private void Update() {
        if (IsMoving) {
            timeMoving += Time.deltaTime;

            if (timeMoving < MaxTimeMoving) {
                // We're between the starting cell and the target cell, keep
                // lerping between them.
                transform.position = Vector3.Lerp(
                    startPositionForMove,
                    endPositionForMove,
                    timeMoving / MaxTimeMoving
                );
            } else {
                // We've made it to the target cell. Either:
                //
                // * Set up the next target cell (if there is one left in the
                //   path), or
                // * End the movement (if we're at the last cell in the path).
                timeMoving = 0.0f;
                MoveToImmediate(targetCoords);
                UseActionPoints(1);

                // TODO: Stop movement if game ended. Maybe here, maybe
                // somewhere more global.
                GameManager.Instance.CheckEndGame();

                PopPathCoords();

                if (targetCoords.x < 0) {
                    // We've moved to the last cell in the path, end the
                    // movement.
                    IsMoving = false;
                    moveCallback?.Invoke();
                }
            }
        }
    }

    public void MoveTo(Vector2Int coords, System.Action callback = null) {
        if (IsMoving) {
            // Don't allow double-moving
            return;
        }

        IsMoving = true;
        timeMoving = 0.0f;
        moveCallback = callback;
        MovementPath = Grid.PathBetween(CurrentCoords, coords, Player.diagonalMoveAllowed);
        remainingMovementPath = new List<Vector2Int>(MovementPath);

        // We're already at the first cell in the path, so remove it.
        remainingMovementPath.RemoveAt(0);

        PopPathCoords();
    }

    public void UseActionPoints(int points) {
        if (points <= ActionPoints) {
            ActionPoints -= points;
            GameManager.Instance.CheckEndGame();
        }
    }

    public void ResetAP() {
        ActionPoints = MaxPoints;
    }

    public void Reset() {
        ResetAP();
        ResetCoords();

        timeMoving = 0.0f;
    }

    private void ResetCoords() {
        MoveToImmediate(new Vector2Int(Grid.Instance.Size.x - 1, Grid.Instance.Size.y - 1));
    }

    private void EndTurn() {
        Turn.Instance.EndTurn();
        ResetAP();
    }

    private Vector3 NormalizedPosition(Vector3 position) {
        return new Vector3(
            position.x,
            transform.position.y,
            position.z
        );
    }

    private void PopPathCoords() {
        if (remainingMovementPath.Count > 0) {
            targetCoords = remainingMovementPath[0];
            remainingMovementPath.RemoveAt(0);

            startPositionForMove = NormalizedPosition(Grid.Instance.PositionForCoords(CurrentCoords));
            endPositionForMove = NormalizedPosition(Grid.Instance.PositionForCoords(targetCoords));
        } else {
            targetCoords = new Vector2Int(-1, -1);
        }
    }

    private void MoveToImmediate(Vector2Int coords) {
        transform.position = NormalizedPosition(Grid.Instance.PositionForCoords(coords));
        CurrentCoords = coords;
    }
}
