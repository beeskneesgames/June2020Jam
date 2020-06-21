using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainIntroController : MonoBehaviour {
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void OnVehicleStopped() {
        Player.Instance.StartMoveAnimation();
    }

    public void OnPlayerWillStop() {
        Player.Instance.StartSkidAnimation();
    }

    public void OnIntroFinished() {
        animator.StopPlayback();
        animator.enabled = false;

        GameManager.Instance.EnableGame();
    }
}
