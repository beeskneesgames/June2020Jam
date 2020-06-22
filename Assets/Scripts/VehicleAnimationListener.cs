using UnityEngine;

public class VehicleAnimationListener : MonoBehaviour {
    public GameObject ultFallPrefab;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void StartUlt() {
        AudioManager.Instance.Play("BigLaunch");
        GameManager.Instance.DisableGameplay();
        animator.SetTrigger("StartUlt");
    }

    public void OnUltOffscreen() {
        Instantiate(ultFallPrefab);
    }
}
