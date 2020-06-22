using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltAnimationListener : MonoBehaviour {
    public void OnExplosionWillEnd() {
        Grid.Instance.FixAllCells();
    }
}
