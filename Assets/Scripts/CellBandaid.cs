using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBandaid : MonoBehaviour {
    private Animator animator;
    private bool shouldShrink;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        if (shouldShrink) {
            animator.SetTrigger("StartShrink");
        }
    }

    public void StartShrink() {
        shouldShrink = true;
    }

    public void OnShrinkWillEnd() {
        Player.Instance.FinishRangedFix();
    }

    public void OnShrinkEnded() {
        Destroy(gameObject);
    }
}
