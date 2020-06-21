using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
    public Vector2Int coords = new Vector2Int(-1, -1);

    private Animator animator;

    private void Start() {
        animator = GetComponentInChildren<Animator>();
    }

    public void Explode() {
        animator.SetTrigger("StartExplode");
    }
}
