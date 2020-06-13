using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour {
    public int spreadRate = 3;
    private List<DamageHead> damageHeads;
    private Vector2Int DefaultHeadCoords1;
    private Vector2Int DefaultHeadCoords2;
    private Vector2Int DefaultHeadCoords3;

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
        DefaultHeadCoords1 = new Vector2Int(0, 0);
        DefaultHeadCoords2 = new Vector2Int(0, Grid.Instance.Size.x - 1);
        DefaultHeadCoords3 = new Vector2Int(Grid.Instance.Size.x - 1, 0);
        damageHeads = new List<DamageHead>();

        AddHead(DefaultHeadCoords1);
        AddHead(DefaultHeadCoords2);
        AddHead(DefaultHeadCoords3);

        Grid.Instance.SetDamageHeads(damageHeads);
    }

    public void Spread() {
        if ((Turn.Instance.TurnCount % spreadRate) == 0) {
            if (damageHeads.Count > 0) {
                foreach (var head in new List<DamageHead>(damageHeads)) {
                    AddHead(head.Coords);
                }
            } else {
                AddHead(DefaultHeadCoords1);
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
