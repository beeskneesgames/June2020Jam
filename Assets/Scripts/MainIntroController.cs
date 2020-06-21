using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainIntroController : MonoBehaviour {
    public void OnVehicleStopped() {
        Player.Instance.StartMoveAnimation();
    }

    public void OnPlayerWillStop() {
        Player.Instance.StartSkidAnimation();
    }
}
