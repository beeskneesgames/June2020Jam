using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructionsController : MonoBehaviour {
    public Button openButton;
    public Button closeButton;
    public GameObject instructionsContainer;
    public TMP_Text instructionsText;

    private void Start() {
        instructionsText.text = string.Join(
            System.Environment.NewLine,
            "The timelines are collapsing into each other! You're OXO, and it's your job to keep your timeline mostly intact until your vehicle charges and finishes the job for you.",
            "",
            "<b>You lose if:</b>",
            "* 50% of the grid falls into the alternate timelines.",
            "* You step into the alternate timelines.",
            "",
            "<b>You win if:</b>",
            $"* You survive {Turn.MaxTurnCount} turns without losing.",
            "* You fix all the cells with timeline damage.",
            "",
            "<b>Your tools:</b>",
            $"These are the tools at your disposal. You only have {Player.Instance.maxPoints} action points every turn, so choose wisely!",
            "",
            $"* Move (1-{Player.Instance.maxPoints} AP): Move to the selected cell. Each cell costs 1 action point. No diagonals and no moving through rocks.",
            $"* Melee fix ({CellInfo.MeleeFixCost} AP): Fix all the cells adjacent to you (including diagonals).",
            $"* Ranged fix ({CellInfo.RangedFixCost} AP): Fix a single cell of your choice within a {ActionManager.RangedFixRange}-cell range (including diagonals).",
            $"* Bomb ({CellInfo.BombCost} AP): Leave a bomb on the selected cell. If the alternate timelines hit the bomb, its cell and all its adjacent cells get fixed (including diagonals).",
            "",
            "<b>How the alternate timelines work</b>",
            $"Every turn, the alternate timeline \"heads\" move to a new cell and consume it. Every {DamageManager.Instance.spreadRate} turns, each head splits into two. Watch out for exponential growth!"
        );
    }

    public void OnOpenButtonClicked() {
        instructionsContainer.SetActive(true);
    }

    public void OnCloseButtonClicked() {
        instructionsContainer.SetActive(false);
    }
}
