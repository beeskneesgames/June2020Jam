using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainIntroController : MonoBehaviour {
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
        AudioManager.Instance.Play("Drive");
    }

    public void OnVehicleStopped() {
        AudioManager.Instance.Stop("Drive");
        AudioManager.Instance.Play("Run");
        Player.Instance.StartMoveAnimation();
    }

    public void OnPlayerWillStop() {
        Player.Instance.StartSkidAnimation();
        AudioManager.Instance.Stop("Run");
        AudioManager.Instance.Play("Turn");
    }

    public void OnIntroFinished() {
        AudioManager.Instance.Stop("Turn");

        animator.StopPlayback();
        animator.enabled = false;

        GameManager.Instance.EnableGame();
    }
}
