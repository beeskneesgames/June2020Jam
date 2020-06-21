using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader {
    public static void StartMainScene() {
        SceneManager.LoadScene("MainScene");
    }

    public static void StartTitleScene() {
        SceneManager.LoadScene("TitleScene");
    }

    public static void StartCreditsScene() {
        SceneManager.LoadScene("CreditsScene");
    }

    public static void StartLoseScene() {
        SceneManager.LoadScene("LoseScene");
    }

    public static void ExitGame() {
        Application.Quit();
    }

}
