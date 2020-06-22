using UnityEngine;

public class PlayerBandaid : MonoBehaviour {
    public void OnShootAnimationEnded() {
        Player.Instance.ShootAnimationEnded();
    }
}
