using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour {
    public enum Direction {
        None,
        North, // Moving up the Y axis
        South, // Moving down the Y axis
        East,  // Moving right along the X axis
        West   // Moving left along the X axis
    }

    public static bool diagonalFixAllowed = false;
    public static bool diagonalMoveAllowed = false;

    private static Player instance;
    public static Player Instance {
        get {
            return instance;
        }
    }

    public Animator playerAnimator;

    // Movement
    public bool IsMoving { get; private set; } = false;
    private bool isSkidding = false;
    private bool shouldSkid = false;
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

    // Spinnin, spinnin, spinnin while my hands up
    public Direction CurrentDirection { get; private set; }
    private Direction targetDirection;
    private Vector3 startEulerAnglesForSpin;
    private Vector3 endEulerAnglesForSpin;

    // Action Points
    public TextMeshProUGUI actionPointUI;
    public int maxPoints = 5;
    private int actionPoints;
    public int ActionPoints {
        get {
            return actionPoints;
        }

        set {
            actionPoints = value;
            actionPointUI.text = $"Action Points: {actionPoints}";

            GameManager.Instance.StateChanged();
        }
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start() {
        Reset();
    }

    private void Update() {
        if (IsMoving) {
            timeMoving += Time.deltaTime;

            if (CurrentDirection == targetDirection) {
                TickMove();
            } else {
                TickSpin();
            }
        }
    }

    private void TickSpin() {
        if (timeMoving < MaxTimeMoving) {
            // We're between the starting direction and the target direction,
            // keep lerping between them.
            transform.localEulerAngles = Vector3.Lerp(
                startEulerAnglesForSpin,
                endEulerAnglesForSpin,
                timeMoving / MaxTimeMoving
            );
        } else {
            // We're now facing the correct direction. The next target cell
            // should've already been set up by TickMove().
            timeMoving = 0.0f;
            transform.localEulerAngles = NormalizedEulerAnglesFor(targetDirection);
            CurrentDirection = targetDirection;
        }
    }

    private void TickMove() {
        if (timeMoving < MaxTimeMoving) {
            // We're between the starting cell and the target cell, keep
            // lerping between them.
            float percentComplete = timeMoving / MaxTimeMoving;
            transform.position = Vector3.Lerp(
                startPositionForMove,
                endPositionForMove,
                percentComplete
            );

            // If we're partway between the second to last cell and the last
            // cell, begin the stop-running animation so we skid while moving.
            if (shouldSkid && remainingMovementPath.Count < 1 && percentComplete >= 0.1f && !isSkidding) {
                isSkidding = true;
                playerAnimator.SetTrigger("StartSkid");
            }
        } else {
            // We've made it to the target cell. Do one of these two things:
            //
            // * Set up the next target cell (if there is one left in the
            //   path). This may require a spin before moving.
            // * End the movement (if we're at the last cell in the path).
            timeMoving = 0.0f;
            MoveToImmediate(targetCoords);
            UseActionPoints(1);

            // TODO: Stop movement if game ended. Maybe here, maybe
            // somewhere more global.
            GameManager.Instance.StateChanged();

            PopPathCoords();
            SyncDirection();

            if (targetCoords.x < 0) {
                // We've moved to the last cell in the path, end the
                // movement.
                if (isSkidding) {
                    isSkidding = false;
                } else {
                    playerAnimator.SetTrigger("StopMove");
                }

                IsMoving = false;
                moveCallback?.Invoke();
            }
        }
    }

    private void SyncDirection() {
        // Figure out if we need to do a spin animation before moving.
        Direction newDirection = DirectionBetween(CurrentCoords, targetCoords);

        if (newDirection != CurrentDirection) {
            targetDirection = newDirection;
            startEulerAnglesForSpin = transform.localEulerAngles;
            endEulerAnglesForSpin = EulerAnglesFor(CurrentDirection, targetDirection);
        }
    }

    public void MoveTo(Vector2Int coords, System.Action callback = null) {
        if (IsMoving) {
            // Don't allow double-moving
            return;
        }

        playerAnimator.SetTrigger("StartMove");
        IsMoving = true;
        timeMoving = 0.0f;
        moveCallback = callback;
        MovementPath = Grid.PathBetween(CurrentCoords, coords, Player.diagonalMoveAllowed);
        remainingMovementPath = new List<Vector2Int>(MovementPath);

        // We're already at the first cell in the path, so remove it.
        remainingMovementPath.RemoveAt(0);

        // The player shouldn't skid if they're moving just 1 cell.
        shouldSkid = remainingMovementPath.Count > 1;

        PopPathCoords();
        SyncDirection();
    }

    public void UseActionPoints(int points) {
        if (points <= ActionPoints) {
            ActionPoints -= points;
        }
    }

    public void ResetAP() {
        ActionPoints = maxPoints;
    }

    public void Reset() {
        ResetAP();
        ResetCoords();

        CurrentDirection = Direction.South;
        targetDirection = Direction.South;
        transform.localEulerAngles = new Vector3(0, 90.0f, 0);

        timeMoving = 0.0f;
        IsMoving = false;
        isSkidding = false;
        shouldSkid = false;
    }

    private void ResetCoords() {
        MoveToImmediate(new Vector2Int(Grid.Instance.Size.x - 1, Grid.Instance.Size.y - 1));
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

    private static Direction DirectionBetween(Vector2Int startCoords, Vector2Int endCoords) {
        if (startCoords == endCoords) {
            // If the start and end coords are the same, there's no direction
            // between them.
            return Direction.None;
        } else if (startCoords.x == endCoords.x) {
            // If the start and end have the same X coords, we're moving north
            // or south.
            if (startCoords.y < endCoords.y) {
                return Direction.North;
            } else {
                return Direction.South;
            }
        } else if (startCoords.y == endCoords.y) {
            // If the start and end have the same Y coords, we're moving east or
            // west.
            if (startCoords.x < endCoords.x) {
                return Direction.East;
            } else {
                return Direction.West;
            }
        } else {
            // If the start and end coords are on a different row AND column,
            // we'd have to be facing somewhere other than one of the 4 cardinal
            // directions, which we don't support.
            return Direction.None;
        }
    }

    private static Vector3 NormalizedEulerAnglesFor(Direction direction) {
        return EulerAnglesFor(Direction.South, direction);
    }

    private static Vector3 EulerAnglesFor(Direction startDirection, Direction endDirection) {
        switch (endDirection) {
            case Direction.North:
                if (startDirection == Direction.East) {
                    return new Vector3(0.0f, -90.0f, 0.0f);
                } else {
                    return new Vector3(0.0f, 270.0f, 0.0f);
                }
            case Direction.South:
                return new Vector3(0.0f, 90.0f, 0.0f);
            case Direction.East:
                if (startDirection == Direction.North) {
                    return new Vector3(0.0f, 360.0f, 0.0f);
                } else {
                    return Vector3.zero;
                }
            case Direction.West:
                return new Vector3(0.0f, 180.0f, 0.0f);
            default:
                return new Vector3(0.0f, 90.0f, 0.0f);
        }
    }
}
