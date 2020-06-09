using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour {
    private static DamageManager instance;
    private List<DamageHead> damageHeads = new List<DamageHead>();

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

        Grid.Instance.SetDamageHeads(damageHeads);
    }

    public void Spread() {
        if (damageHeads.Count == 0) {
            // Add head in an available spot
        }

        if ((Turn.Instance.TurnCount % 3) == 0) {
            AddHead(damageHeads[UnityEngine.Random.Range(0, damageHeads.Count)].Coords);
        }

        foreach (DamageHead damageHead in damageHeads) {
            damageHead.Move();
        }

        Grid.Instance.SetDamageHeads(damageHeads);
    }

    private void AddHead(Vector2Int coords) {
        DamageHead damageHead = new DamageHead(coords);

        damageHeads.Add(damageHead);
        Grid.Instance.DamageCell(coords);
    }
}
