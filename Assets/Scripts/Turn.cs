﻿using TMPro;
using UnityEngine;

public class Turn : MonoBehaviour {
    private static Turn instance;
    private int turnCount;

    public const int MaxTurnCount = 25;
    public TextMeshProUGUI turnUI;

    public int TurnCount {
        get {
            return turnCount;
        }
        private set {
            int oldTurnCount = turnCount;
            turnCount = value;
            turnUI.text = $"TURN {turnCount + 1}";

            if (oldTurnCount < turnCount) {
                DamageManager.Instance.Spread();
            }

            GameManager.Instance.StateChanged();
        }
    }

    public static Turn Instance {
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
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        Reset();        
    }

    // This should only be called by GameManager in LateUpdate.
    public void End() {
        Player.Instance.ResetAP();
        TurnCount++;
    }

    public void Reset() {
        TurnCount = 0;
    }
}
