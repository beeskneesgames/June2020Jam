using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBandaid : MonoBehaviour {
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    public void StartFall(Vector3 cellPosition) {
        transform.position = new Vector3(
            cellPosition.x,
            transform.position.y,
            cellPosition.z
        );
        animator.SetTrigger("StartFall");
    }
}
