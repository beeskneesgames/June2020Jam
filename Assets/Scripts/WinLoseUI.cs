using TMPro;
using UnityEngine;

public class WinLoseUI : MonoBehaviour {
    public GameObject losePanel;

    public void Show(bool win) {
        SetWinLose(win);

        AudioManager.Instance.Stop("Theme");
    }

    public void Hide() {
        losePanel.SetActive(false);
    }

    public void SetWinLose(bool win) {
        if (win) {
            AudioManager.Instance.Play("Win");
        } else {
            AudioManager.Instance.Play("Lose");
            losePanel.SetActive(true);
        }
    }

    public void ResetBoard() {
        AudioManager.Instance.Stop("Win");
        AudioManager.Instance.Stop("Lose");
        AudioManager.Instance.Play("Theme");
        GameManager.Instance.Reset();
        Hide();
    }
}
