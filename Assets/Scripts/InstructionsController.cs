using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsController : MonoBehaviour {
    public Button openButton;
    public Button closeButton;
    public GameObject instructionsContainer;

    public void OnOpenButtonClicked() {
        instructionsContainer.SetActive(true);
    }

    public void OnCloseButtonClicked() {
        instructionsContainer.SetActive(false);
    }
}
