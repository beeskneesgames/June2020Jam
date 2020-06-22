using System;
using System.Collections;
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
    public static Vector2Int StartCoords {
        get {
            return Grid.Instance.Size - new Vector2Int(1, 1);
        }
    }

    private const float StartY = -1.49f;

    private static Player instance;
    public static Player Instance {
        get {
            return instance;
        }
    }

    public Animator playerAnimator;
    public Animator backpackBandaidAnimator;
    public RangedBandaid rangedBandaid;
    public GameObject cellBandaidPrefab;
    public GameObject meleeBandaidPrefab;
    public GameObject bombPrefab;

    public Vector2Int CurrentCoords { get; private set; }

    public CellInfo CurrentCell {
        get {
            return Grid.Instance.CellInfoAt(CurrentCoords);
        }
    }

    // Movement
    public bool IsPerformingAction { get; private set; } = false;
    public List<Vector2Int> MovementPath { get; private set; }
    private const float MaxTimeMoving = 0.25f;
    private Coroutine moveCoroutine;

    // Spinnin, spinnin, spinnin while my hands up
    public Direction CurrentDirection { get; private set; }

    // Fixes
    private bool meleeFixInProgress = false;
    private Vector2Int rangedFixCoords = new Vector2Int(-1, -1);
    private List<Bomb> bombs = new List<Bomb>();

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
            actionPointUI.text = $"Action Points: {actionPoints}/{maxPoints}";

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
        // We don't fully call Reset() here because the player starts out in an
        // animation.
        ResetAP();
    }

    public void MoveTo(Vector2Int endCoords, System.Action callback = null) {
        if (IsPerformingAction) {
            // Don't allow double-moving
            return;
        }

        IsPerformingAction = true;
        moveCoroutine = StartCoroutine(PerformMoveTo(endCoords, callback));
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
        transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);

        rangedFixCoords = new Vector2Int(-1, -1);
        meleeFixInProgress = false;

        foreach (var bomb in bombs) {
            Destroy(bomb.gameObject);
        }
        bombs.Clear();

        if (moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }
    }

    public void StartShootAnimation() {
        playerAnimator.SetTrigger("FixRanged");
        backpackBandaidAnimator.SetTrigger("StartShoot");
    }

    public void StartMoveAnimation() {
        playerAnimator.SetTrigger("StartMove");
    }

    public void RangedFix(Vector2Int coords) {
        IsPerformingAction = true;
        rangedFixCoords = coords;
        StartShootAnimation();
    }

    public void ShootAnimationEnded() {
        rangedBandaid.StartFall(Grid.Instance.PositionForCoords(rangedFixCoords));
    }

    public void FallAnimationEnded() {
        Vector3 cellPosition = Grid.Instance.PositionForCoords(rangedFixCoords);

        GameObject cellBandaid = Instantiate(cellBandaidPrefab);
        cellBandaid.transform.position = new Vector3(
            cellPosition.x,
            0.0f,
            cellPosition.z
        );
        cellBandaid.transform.localEulerAngles = new Vector3(
            cellBandaid.transform.localEulerAngles.x,
            UnityEngine.Random.Range(0.0f, 360.0f),
            cellBandaid.transform.localEulerAngles.z
        );
        cellBandaid.GetComponent<CellBandaid>().StartShrink();
    }

    public void FinishRangedFix() {

        Grid.Instance.CellInfoAt(rangedFixCoords).RangedFix();
        rangedFixCoords = new Vector2Int(-1, -1);
        IsPerformingAction = false;
    }

    internal void MeleeFix() {
        IsPerformingAction = true;
        meleeFixInProgress = true;
        playerAnimator.SetTrigger("FixMelee");

        foreach (var cell in Grid.Instance.AdjacentTo(Player.Instance.CurrentCoords, true)) {
            GameObject meleeBandaid = Instantiate(meleeBandaidPrefab);
            Vector3 cellPosition = Grid.Instance.PositionForCoords(cell.Coords);
            meleeBandaid.transform.position = new Vector3(
                cellPosition.x,
                meleeBandaid.transform.position.y,
                cellPosition.z
            );
        }
    }

    public void FinishMeleeFix() {
        // Make sure we don't fix multiple times from one melee fix command.
        if (!meleeFixInProgress) {
            return;
        }

        Grid.Instance.CellInfoAt(Player.Instance.CurrentCoords).MeleeFix();
        IsPerformingAction = false;
        meleeFixInProgress = false;
    }

    public void PlaceBomb(Vector2Int coords) {
        Grid.Instance.CellInfoAt(coords).AddBomb();

        Bomb bomb = Instantiate(bombPrefab).GetComponent<Bomb>(); // bi dom bi dum bum bay
        bomb.coords = coords;
        bomb.transform.position = Grid.Instance.PositionForCoords(coords);
        bombs.Add(bomb);
    }

    public void ExplodeBombAt(CellInfo cell) {
        Bomb bombToExplode = null;

        foreach (var bomb in bombs) {
            if (bomb.coords == cell.Coords) {
                bombToExplode = bomb;
                break;
            }
        }

        if (bombToExplode) {
            bombs.Remove(bombToExplode);
            bombToExplode.Explode();
        }
    }

    public void StartSkidAnimation() {
        playerAnimator.SetTrigger("StartSkid");
    }

    public void EnterGame() {
        transform.parent = null;
        transform.position = new Vector3(0.0f, StartY, 0.0f);
        Reset();
        GameManager.Instance.StateChanged();
    }

    private IEnumerator PerformMoveTo(Vector2Int endCoords, System.Action callback = null) {
        AudioManager.Instance.Play("Run");
        StartMoveAnimation();

        MovementPath = Grid.PathBetween(CurrentCoords, endCoords, diagonalMoveAllowed);

        // The player shouldn't skid if they're moving just 1 cell (which would
        // have a movement path count of 2: the start cell and the end cell).
        bool shouldSkid = MovementPath.Count > 2;

        // Start at 1, since the player is already at the first cell in the path.
        for (int i = 1; i < MovementPath.Count; i++) {
            Vector2Int coords = MovementPath[i];

            // Spin the player until they're facing the coords they'll be moving
            // towards.
            yield return StartCoroutine(SpinToFaceNextCoords(coords));

            // Move the player to the next coords.
            yield return StartCoroutine(MoveToNextCoords(coords, shouldSkid, i == (MovementPath.Count - 1)));

            UseActionPoints(1);

            // TODO: Stop movement if game ended. Maybe here, maybe
            // somewhere more global.
            GameManager.Instance.StateChanged();
        }

        // If we didn't finish the movement with a skid, stop the move.
        if (!shouldSkid) {
            AudioManager.Instance.Stop("Run");
            playerAnimator.SetTrigger("StopMove");
        }

        IsPerformingAction = false;
        MovementPath = null;
        Grid.Instance.ClearPath();
        callback?.Invoke();
    }

    private IEnumerator SpinToFaceNextCoords(Vector2Int coords) {
        // Figure out if we need to do a spin animation before moving.
        Direction targetDirection = DirectionBetween(CurrentCoords, coords);

        if (targetDirection != CurrentDirection) {
            float timeSpinning = 0.0f;
            Vector3 startEulerAngles = transform.localEulerAngles;
            Vector3 endEulerAngles = EulerAnglesFor(CurrentDirection, targetDirection);

            // Spin until we're facing the correct direction.
            while (timeSpinning < MaxTimeMoving) {
                timeSpinning += Time.deltaTime;

                transform.localEulerAngles = Vector3.Lerp(
                    startEulerAngles,
                    endEulerAngles,
                    timeSpinning / MaxTimeMoving
                );

                // Give up control of execution until the next frame.
                yield return null;
            }

            // We're now facing the correct direction. Normalize the euler
            // angles so we don't end up twisted outside the 0-360 degree
            // range.
            transform.localEulerAngles = NormalizedEulerAnglesFor(targetDirection);
            CurrentDirection = targetDirection;
        }
    }

    private IEnumerator MoveToNextCoords(Vector2Int coords, bool shouldSkid, bool isLastCoords) {
        bool isSkidding = false;
        float timeMoving = 0.0f;
        Vector3 startPosition = NormalizedPosition(Grid.Instance.PositionForCoords(CurrentCoords));
        Vector3 endPosition = NormalizedPosition(Grid.Instance.PositionForCoords(coords));

        // Move until we're on the target cell
        while (timeMoving < MaxTimeMoving) {
            timeMoving += Time.deltaTime;
            float percentComplete = timeMoving / MaxTimeMoving;
            transform.position = Vector3.Lerp(
                startPosition,
                endPosition,
                percentComplete
            );

            // Start skid if necessary.
            //
            // We start the skid if all the following things are true:
            // 1. The player is traveling more than 1 cell.
            // 2. The player is more than 10% of the way between the
            //    second-to-last and the last cell in their path.
            // 3. The skid has not already been started.
            if (shouldSkid && !isSkidding && isLastCoords && percentComplete > 0.1f) {
                AudioManager.Instance.Stop("Run");
                AudioManager.Instance.Play("Skid");
                StartSkidAnimation();
                isSkidding = true;
            }

            yield return null;
        }

        MoveToImmediate(coords);
    }

    private void ResetCoords() {
        MoveToImmediate(StartCoords);
    }

    private Vector3 NormalizedPosition(Vector3 position) {
        return new Vector3(
            position.x,
            transform.position.y,
            position.z
        );
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
