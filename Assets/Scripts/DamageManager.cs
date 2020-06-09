using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour {
    private static DamageManager instance;
    private List<DamageHead> damageHeads;

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
        AddHead(new Vector2Int(0,0));
    }

    public void Spread() {
        foreach (DamageHead damageHead in damageHeads) {
            damageHead.Move();
        }
    }

    private void AddHead(Vector2Int coords) {
        DamageHead damageHead = new DamageHead();
        damageHead.coords = coords;

        damageHeads.Add(damageHead);
    }
}
