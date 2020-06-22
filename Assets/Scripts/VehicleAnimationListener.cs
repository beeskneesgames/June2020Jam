using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAnimationListener : MonoBehaviour {
    public GameObject ultFallPrefab;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void StartUlt() {
        animator.SetTrigger("StartUlt");
    }

    public void OnUltOffscreen() {
        Instantiate(ultFallPrefab);
    }
}
