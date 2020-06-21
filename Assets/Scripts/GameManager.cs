﻿using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private const float LossPercent = 0.5f;

    public WinLoseUI winLoseUI;
    public GameObject gameUI;
    public GameObject actionUI;
    public GameObject debugUI;

    private bool stateChanged = false;

    private static GameManager instance;
    public static GameManager Instance {
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
        AudioManager.Instance.Stop("Intro");
        AudioManager.Instance.Play("Theme");
    }

    public void EnableGame() {
        gameUI.SetActive(true);
        actionUI.SetActive(true);
        debugUI.SetActive(Debug.isDebugBuild);

        Player.Instance.EnterGame();
    }

    private void LateUpdate() {
        if (stateChanged) {
            if (Turn.Instance.TurnCount > 0) {
                CheckEndGame();
            }

            if (Player.Instance.ActionPoints < 1) {
                Turn.Instance.End();
                CheckEndGame();
            }

            stateChanged = false;
        }
    }

    public void Reset() {
        Player.Instance.Reset();
        Turn.Instance.Reset();
        Grid.Instance.Reset();
        DamageManager.Instance.Reset();
        ObstacleManager.Instance.Reset();
        ActionManager.Instance.Reset();
    }

    public void StateChanged() {
        stateChanged = true;
    }

    private void CheckEndGame() {
        if (CheckForLoss()) {
            TriggerLoss();
        } else if (CheckForWin()) {
            TriggerWin();
        }
    }

    private bool CheckForLoss() {
        if (Grid.Instance.PercentDamaged() >= LossPercent) {
            return true;
        }

        if (Player.Instance.CurrentCell.IsDamaged) {
            return true;
        }

        return false;
    }

    private bool CheckForWin() {
        if (Turn.Instance.TurnCount >= Turn.MaxTurnCount) {
            return true;
        }

        if (!Grid.Instance.HasDamage && Turn.Instance.TurnCount > 1) {
            return true;
        }

        return false;
    }

    private void TriggerWin() {
        winLoseUI.Show(true);
    }

    private void TriggerLoss() {
        winLoseUI.Show(false);
    }
}
