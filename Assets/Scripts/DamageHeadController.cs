using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHeadController : MonoBehaviour {
    public GameObject model;

    public void DestroyGameObject() {
        Destroy(gameObject);
    }
}
