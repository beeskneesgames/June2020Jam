using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    public Vector2Int Coords { get; set; } = new Vector2Int(-1, -1);

    private void Start() {
        Generate();
    }

    private void Generate() {

    }
}
