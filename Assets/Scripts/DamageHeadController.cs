using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHeadController : MonoBehaviour {
    public GameObject model;

    private void Start() {
        model.transform.localEulerAngles = new Vector3(
            model.transform.localEulerAngles.x,
            model.transform.localEulerAngles.y,
            Random.Range(0.0f, 360.0f)
        );
    }
}
