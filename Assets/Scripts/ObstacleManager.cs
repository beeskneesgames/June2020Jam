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
        CellInfo towerCell1;
        CellInfo[] towerCells;
        do {
            towerCell1 = Grid.Instance.RetrieveRandomCell(2);
            towerCells = Grid.Instance.AdjacentTo(towerCell1.Coords, true);
        } while (towerCells.Length < 3);

        towerCell1 = Grid.Instance.RetrieveRandomCell(2);
        towerCells = Grid.Instance.AdjacentTo(towerCell1.Coords, true);

        CellInfo towerCell2 = towerCells[0];
        CellInfo towerCell3 = towerCells[1];
        CellInfo towerCell4 = towerCells[2];

        towerCell1.AddObstacle();
        towerCell2.AddObstacle();
        towerCell3.AddObstacle();
        towerCell4.AddObstacle();
    }
}
