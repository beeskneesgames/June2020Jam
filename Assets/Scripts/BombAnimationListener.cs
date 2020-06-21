using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAnimationListener : MonoBehaviour {
    public GameObject rootBombObject;

    public void OnBombExploded() {
        Destroy(rootBombObject);
    }
}
