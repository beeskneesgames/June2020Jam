﻿using System.Collections;
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

    // Movement
    private bool isMoving = false;
    private System.Action moveCallback;
    private Vector2Int currentCoords = new Vector2Int(7, 7);
    private Vector2Int targetCoords = new Vector2Int(-1, -1);
    private Vector3 startPositionForMove;
    private Vector3 endPositionForMove;
    private float timeMoving = 0.0f;
    private const float MaxTimeMoving = 1.0f;

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

    private void Update() {
        if (isMoving) {
            timeMoving += Time.deltaTime;

            if (timeMoving < MaxTimeMoving) {
                transform.position = Vector3.Lerp(
                    startPositionForMove,
                    endPositionForMove,
                    timeMoving / MaxTimeMoving
                );
            } else {
                isMoving = false;
                timeMoving = 0.0f;
                transform.position = endPositionForMove;
                currentCoords = targetCoords;

                moveCallback?.Invoke();

                GameManager.Instance.CheckEndGame();
            }
        }
    }

    public void MoveTo(Vector2Int coords, System.Action callback = null) {
        Debug.Log("In Player.MoveTo");
        if (isMoving) {
            // Don't allow double-moving
            return;
        }

        isMoving = true;
        moveCallback = callback;
        targetCoords = coords;

        startPositionForMove = NormalizedPosition(Grid.Instance.PositionForCoords(currentCoords));
        endPositionForMove = NormalizedPosition(Grid.Instance.PositionForCoords(currentCoords));

        GameManager.Instance.CheckEndGame();
    }

    private void EndTurn() {
        Turn.Instance.EndTurn();
        ActionPoints = MaxPoints;
    }

    private Vector3 NormalizedPosition(Vector3 position) {
        return new Vector3(
            position.x,
            transform.position.y,
            position.z
        );
    }
}
