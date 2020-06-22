using UnityEngine;

public class TitleMenu : MonoBehaviour {
    public void Start() {
        AudioManager.Instance.Stop("Drive");
        AudioManager.Instance.Stop("Run");
        AudioManager.Instance.Stop("Skid");

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
