using TMPro;
using UnityEngine;

public class WinLoseUI : MonoBehaviour {
    public GameObject panel;
    public TextMeshProUGUI winLoseText;

    public void Show(bool win) {
        SetWinLose(win);
        panel.SetActive(true);

        AudioManager.Instance.Stop("Theme");
    }

    public void Hide() {
        panel.SetActive(false);
    }

    public void SetWinLose(bool win) {
        if (win) {
            AudioManager.Instance.Play("Win");
            winLoseText.text = "You win!";
        } else {
            AudioManager.Instance.Play("Lose");
            winLoseText.text = "You lose :(";
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
