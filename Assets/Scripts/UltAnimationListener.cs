using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltAnimationListener : MonoBehaviour {
    public void OnFallEnded() {
        transform.parent.position = new Vector3(0.0f, -11f, 0.0f);
    }

    public void OnExplosionWillEnd() {
        Grid.Instance.FixAllCells();
        StartCoroutine(ShowWinScreenAfterDelay());
    }

    public IEnumerator ShowWinScreenAfterDelay() {
        yield return new WaitForSeconds(1.0f);
        SceneLoader.StartWinScene();
    }
}
