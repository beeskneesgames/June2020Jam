using UnityEngine;

public class EndTurnButton : MonoBehaviour {
    public ActionMenu actionMenu;

    public void EndTurn() {
        Grid.Instance.ClearActionArea();
        Player.Instance.UseActionPoints(Player.Instance.ActionPoints);
    }
}
