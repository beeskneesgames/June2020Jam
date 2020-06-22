using UnityEngine;

public class Credits : MonoBehaviour {
    public void ExitCredits() {
        SceneLoader.StartTitleScene();
    }

    public void OnButtonClick() {
        AudioManager.Instance.PlayBtnClick();
    }

    public void OnButtonHover() {
        AudioManager.Instance.PlayBtnHover();
    }
}
