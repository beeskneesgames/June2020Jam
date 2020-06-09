using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour {
    private static DamageManager instance;
    private List<Vector2Int> damageHeads;

    public static DamageManager Instance {
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

    private void Start() {
        AddHead();
    }

    private void AddHead() {
        Vector2Int damageHead = new Vector2Int(-1, -1);

        damageHeads.Add(damageHead);
    }
}
