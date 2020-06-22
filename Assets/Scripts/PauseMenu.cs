using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public GameObject menuContainer;

    private float oldTimeScale;

    private void Start() {
        oldTimeScale = Time.timeScale;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Globals.isPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    private void OnDestroy() {
        Resume();
    }

    public void Resume() {
        AudioManager.Instance.Unmute("Drive");
        AudioManager.Instance.Unmute("Run");
        AudioManager.Instance.Unmute("Skid");

        Globals.isPaused = false;
        Time.timeScale = oldTimeScale;

        menuContainer.SetActive(false);
    }

    public void Pause() {
        AudioManager.Instance.Mute("Drive");
        AudioManager.Instance.Mute("Run");
        AudioManager.Instance.Mute("Skid");

        Globals.isPaused = true;
        oldTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;

        menuContainer.SetActive(true);
    }

    public void ExitGame() {
        Time.timeScale = oldTimeScale;
        GameManager.Instance.Reset();

        SceneLoader.StartTitleScene();
    }
}
