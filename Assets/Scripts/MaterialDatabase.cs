using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialDatabase : MonoBehaviour {
    public Material cellDamaged;
    public Material cellHealthy;

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
