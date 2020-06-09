﻿using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour {
    private static DamageManager instance;
    private const int SpreadRate = 3;
    private List<DamageHead> damageHeads = new List<DamageHead>();
    private Vector2Int DefaultHeadCoords = new Vector2Int(0, 0);

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
        AddHead(DefaultHeadCoords);

        Grid.Instance.SetDamageHeads(damageHeads);
    }

    public void Spread() {
        if ((Turn.Instance.TurnCount % SpreadRate) == 0) {
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
        Debug.Log($"damageHeads before: {damageHeads}");
        damageHeads.RemoveAll(head => head.Coords == coords);
        Debug.Log($"damageHeads after: {damageHeads}");
    }

    private void AddHead(Vector2Int coords) {
        damageHeads.Add(new DamageHead(coords));
    }
}
