using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBandaid : MonoBehaviour {
    public void OnShootAnimationEnded() {
        Player.Instance.ShootAnimationEnded();
    }
}
