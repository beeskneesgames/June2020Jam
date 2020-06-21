using UnityEngine;

public class TitleMenu : MonoBehaviour {
    public void Start() {
        if (!AudioManager.Instance.IntroPlaying) {
            AudioManager.Instance.Play("Intro");
        }
    }

    public void StartGame() {
        SceneLoader.StartMainScene();
    }

    public void StartCredits() {
        SceneLoader.StartCreditsScene();
    }

    public void ExitGame() {
        SceneLoader.ExitGame();
    }
}
