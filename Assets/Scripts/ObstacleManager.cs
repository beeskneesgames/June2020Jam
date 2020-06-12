using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {
    public Vector2Int Coords { get; set; } = new Vector2Int(-1, -1);
    private int smallCount = 2;
    private int bigCount = 1;

    private void Start() {
        Generate();
    }

    private void Generate() {
        // Generate small obstacles
        for (int count = 0; count < smallCount; count++) {
            CellInfo cell = Grid.Instance.RetrieveRandomCell();

            cell.AddObstacle();
        }

        // Generate big obstacles
        for (int count = 0; count < bigCount; count++) {
            CellInfo cell1 = Grid.Instance.RetrieveRandomCell(2);
            CellInfo[] bigObstacleCells = Grid.Instance.AdjacentTo(cell1.Coords, false);

            while (bigObstacleCells.Length < 2) {
                cell1 = Grid.Instance.RetrieveRandomCell(2);
                bigObstacleCells = Grid.Instance.AdjacentTo(cell1.Coords, false);
            }

            CellInfo cell2 = bigObstacleCells[0];

            cell1.AddObstacle();
            cell2.AddObstacle();
        }

        // Generate tower
        CellInfo[] towerCells = new CellInfo[4];
        bool overlap;

        do {
            towerCells[0] = Grid.Instance.RetrieveRandomCell(1);
            towerCells[1] = Grid.Instance.CellInfoAt(towerCells[0].Coords + new Vector2Int(1, 0));
            towerCells[2] = Grid.Instance.CellInfoAt(towerCells[0].Coords + new Vector2Int(0, 1));
            towerCells[3] = Grid.Instance.CellInfoAt(towerCells[0].Coords + new Vector2Int(1, 1));

            overlap = false;

            // Make sure the tower doesn't overlap any of the other obstacles.
            foreach (var towerCell in towerCells) {
                if (towerCell.HasObstacle) {
                    overlap = true;
                    break;
                }
            }
        } while (overlap);

        foreach (var towerCell in towerCells) {
            towerCell.AddObstacle();
        }
    }
}
