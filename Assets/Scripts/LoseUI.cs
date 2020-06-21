using UnityEngine;

public class LoseUI : MonoBehaviour {

    public void Start() {
        AudioManager.Instance.Stop("Theme");
        AudioManager.Instance.Play("Lose");
    }

    public void ResetBoard() {
        AudioManager.Instance.Stop("Lose");

        SceneLoader.StartMainScene();
    }
}
