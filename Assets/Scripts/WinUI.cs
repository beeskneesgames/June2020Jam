using UnityEngine;

public class WinUI : MonoBehaviour {

    public void Start() {
        AudioManager.Instance.Stop("Theme");
        AudioManager.Instance.Play("Win");
    }

    public void ResetBoard() {
        AudioManager.Instance.Stop("Win");

        SceneLoader.StartMainScene();
    }

    public void OnButtonClick() {
        AudioManager.Instance.PlayBtnClick();
    }

    public void OnButtonHover() {
        AudioManager.Instance.PlayBtnHover();
    }
}
