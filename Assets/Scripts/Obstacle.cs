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

    private MeshFilter meshFilter;

    private void Start() {
        meshFilter = GetComponentInChildren<MeshFilter>();
        SyncMesh();
    }

    private void SyncMesh() {
        if (meshFilter != null) {
            meshFilter.mesh = Mesh;
        }
    }
}
