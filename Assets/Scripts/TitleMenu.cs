using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour {
    public VehicleTitleController vehicle;

    private Button[] buttons;

    public void Start() {
        AudioManager.Instance.Stop("Drive");
        AudioManager.Instance.Stop("Run");
        AudioManager.Instance.Stop("Skid");

        if (!AudioManager.Instance.IntroPlaying) {
            AudioManager.Instance.Play("Intro");
        }

        buttons = GetComponentsInChildren<Button>();
    }

    public void StartGame() {
        // Disable all buttons so they aren't clicked during the animation
        foreach (var button in buttons) {
            button.interactable = false;
        }

        vehicle.StartMove();
    }

    public void StartCredits() {
        SceneLoader.StartCreditsScene();
    }

    public void ExitGame() {
        SceneLoader.ExitGame();
    }
}
