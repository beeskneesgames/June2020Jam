using System.Collections;
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
        AddHead();
    }

    public void Spread() {
        foreach (DamageHead damageHead in damageHeads) {
            damageHead.Move();
        }
    }

    private void AddHead() {
        DamageHead damageHead = new DamageHead();

        damageHeads.Add(damageHead);
    }
}
