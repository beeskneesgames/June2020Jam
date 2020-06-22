using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleTitleController : MonoBehaviour {
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void StartMove() {
        animator.SetTrigger("StartMove");
    }

    public void OnMovedOffscreen() {
        SceneLoader.StartMainScene();
    }
}
