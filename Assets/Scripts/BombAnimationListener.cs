using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAnimationListener : MonoBehaviour {
    public GameObject rootBombObject;

    public void OnBombExploded() {
        Destroy(rootBombObject);
    }

    public void OnExplosionWillEnd() {
        // Do nothing, since this method name is meant for the ult.
    }
}
