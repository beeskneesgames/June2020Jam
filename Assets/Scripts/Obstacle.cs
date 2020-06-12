using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    public Vector2Int Coords { get; set; } = new Vector2Int(-1, -1);
    private int smallCount = 2;
    private int bigCount = 1;

    private void Start() {
        Generate();
    }

    private void Generate() {
        // Generate small obstacles
        for (int count = 0; count < smallCount; count++) {
        }

        // Generate big obstacles
        for (int count = 0; count < bigCount; count++) {
        }
    }
}
