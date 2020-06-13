using TMPro;
using UnityEngine;

public class WinLoseUI : MonoBehaviour {
    public GameObject panel;
    public TextMeshProUGUI winLoseText;

    public void Show(bool win) {
        SetWinLoseText(win);
        panel.SetActive(true);
    }

    public void Hide() {
        panel.SetActive(false);
    }

    public void SetWinLoseText(bool win) {
        if (win) {
            winLoseText.text = "You win!";
        } else {
            winLoseText.text = "You lose :(";
        }
    }

    public void ResetBoard() {
        GameManager.Instance.Reset();
        Hide();
    }
}
