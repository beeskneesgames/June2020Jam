using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private const float LossPercent = 0.5f;

    public GameObject gameUI;
    public GameObject actionUI;
    public GameObject debugUI;
    public VehicleAnimationListener vehicle;

    private bool stateChanged = false;

    public bool IsGameplayEnabled { get; private set; } = false;

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

    private void OnDestroy() {
        AudioManager.Instance.Stop("Theme");
    }

    public void EnableGameplay() {
        gameUI.SetActive(true);
        actionUI.SetActive(true);
        debugUI.SetActive(Debug.isDebugBuild);

        Player.Instance.EnterGame();

        IsGameplayEnabled = true;
    }

    public void DisableGameplay() {
        IsGameplayEnabled = false;

        gameUI.SetActive(false);
        actionUI.SetActive(false);
        debugUI.SetActive(false);
    }

    private void LateUpdate() {
        if (IsGameplayEnabled && stateChanged) {
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
        vehicle.StartUlt();
    }

    private void TriggerLoss() {
        SceneLoader.StartLoseScene();
    }
}
