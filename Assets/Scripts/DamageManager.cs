using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour {
    public int spreadRate = 3;
    private List<DamageHead> damageHeads;
    private Vector2Int DefaultHeadCoords = new Vector2Int(0, 0);

    private static DamageManager instance;
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
        Reset();
    }

    public void Reset() {
        damageHeads = new List<DamageHead>();

        AddHead(DefaultHeadCoords);

        Grid.Instance.SetDamageHeads(damageHeads);
    }

    public void Spread() {
        if ((Turn.Instance.TurnCount % spreadRate) == 0) {
            if (damageHeads.Count > 0) {
                AddHead(damageHeads[Random.Range(0, damageHeads.Count)].Coords);
            } else {
                AddHead(DefaultHeadCoords);
            }
        }

        foreach (DamageHead damageHead in damageHeads) {
            damageHead.Move();
        }

        Grid.Instance.SetDamageHeads(damageHeads);
    }

    public void RemoveHeadsAt(Vector2Int coords) {
        damageHeads.RemoveAll(head => head.Coords == coords);
    }

    private void AddHead(Vector2Int coords) {
        damageHeads.Add(new DamageHead(coords));
    }
}
