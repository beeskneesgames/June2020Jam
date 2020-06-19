using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    private Mesh mesh = null;
    public Mesh Mesh {
        get {
            return mesh;
        }

        set {
            mesh = value;
            SyncMesh();
        }
    }

    public Vector2Int SWCoords { get; set; } = new Vector2Int(-1, -1);
    public Vector2Int NECoords { get; set; } = new Vector2Int(-1, -1);

    private MeshFilter meshFilter;

    private void Start() {
        meshFilter = GetComponentInChildren<MeshFilter>();
        SyncMesh();
    }

    public void SetCoords(Vector2Int coords) {
        SetCoords(new List<Vector2Int> { coords });
    }

    public void SetCoords(List<Vector2Int> coordsList) {
        int minX = int.MaxValue;
        int maxX = -1;
        int minY = int.MaxValue;
        int maxY = -1;

        foreach (var coords in coordsList) {
            if (coords.x < minX) {
                minX = coords.x;
            }

            if (coords.x > maxX) {
                maxX = coords.x;
            }

            if (coords.y < minY) {
                minY = coords.y;
            }

            if (coords.y > maxY) {
                maxY = coords.y;
            }
        }

        SWCoords = new Vector2Int(minX, minY);
        NECoords = new Vector2Int(maxX, maxY);
    }

    private void SyncMesh() {
        if (meshFilter != null) {
            meshFilter.mesh = Mesh;
        }
    }
}
