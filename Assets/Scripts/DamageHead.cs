﻿using UnityEngine;

public class DamageHead {
    public Vector2Int coords;

    public Vector2Int Coords {
        get {
            return coords;
        }
        set {
            coords = value;
            GameManager.Instance.CheckEndGame();
        }
    }

    public void Move() {
        //TODO: Move to a random cell that isn't damaged
        coords = new Vector2Int(-1, -1);
    }
}
