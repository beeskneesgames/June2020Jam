using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialDatabase : MonoBehaviour {
    public Material cellDamaged;
    public Material cellHealthy;
    public Material cellHover;
    public Material cellInActionArea;
    public Material cellInPath;

    private static MaterialDatabase instance;
    public static MaterialDatabase Instance {
        get {
            return instance;
        }
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}
