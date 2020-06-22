using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAnimationListener : MonoBehaviour {
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void StartUlt() {
        animator.SetTrigger("StartUlt");
    }

    public void OnUltOffscreen() {
        // TODO: Make falling head
    }
}
