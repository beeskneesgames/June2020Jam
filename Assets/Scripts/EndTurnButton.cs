using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    public void EndTurn() {
        Turn.Instance.EndTurn();
    }
}
