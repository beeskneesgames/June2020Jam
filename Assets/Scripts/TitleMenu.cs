using UnityEngine;

public class TitleMenu : MonoBehaviour {
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
