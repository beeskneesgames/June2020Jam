using UnityEngine;

public class MeleeBandaid : MonoBehaviour {
    public GameObject model;

    public void OnShrinkStarted() {
        model.transform.localEulerAngles = new Vector3(
            -90.0f,
            Random.Range(0.0f, 360.0f),
            90.0f
        );
    }

    public void OnShrinkWillEnd() {
        Player.Instance.FinishMeleeFix();
    }

    public void OnShrinkEnded() {
        Destroy(gameObject);
    }
}
