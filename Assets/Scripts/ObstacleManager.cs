using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {
    private readonly int smallCount = 2;
    private readonly int bigCount = 1;
    private List<CellInfo> obstacleCells = new List<CellInfo>();
    private List<Obstacle> obstacles = new List<Obstacle>();

    public GameObject obstaclePrefab;
    public GameObject smallRockPrefab;
    public GameObject bigRockPrefab;
    public GameObject holePrefab;

    private static ObstacleManager instance;
    public static ObstacleManager Instance {
        get {
            return instance;
        }
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start() {
        Reset();
    }

    private void Generate() {
        // Generate small obstacles
        for (int count = 0; count < smallCount; count++) {
            CellInfo cell;
            do {
                cell = Grid.Instance.RetrieveRandomCell();
            } while (!IsValidForObstacles(cell));

            cell.AddObstacle();
            obstacleCells.Add(cell);

            Obstacle smallRock = Instantiate(obstaclePrefab, Grid.Instance.transform).GetComponent<Obstacle>();
            smallRock.CurrentType = Obstacle.Type.SmallRock;
            smallRock.SetCoords(cell.Coords);
            obstacles.Add(smallRock);
        }

        // Generate big obstacles
        for (int count = 0; count < bigCount; count++) {
            List<CellInfo> bigObstacleCells = new List<CellInfo>(2);

            do {
                bigObstacleCells.Clear();
                bigObstacleCells.Add(Grid.Instance.RetrieveRandomCell());

                List<CellInfo> adjacentCells = Grid.Instance.AdjacentTo(bigObstacleCells[0].Coords, false);
                bigObstacleCells.Add(adjacentCells[Random.Range(0, adjacentCells.Count)]);
            } while (!IsValidForObstacles(bigObstacleCells));

            foreach (var bigObstacleCell in bigObstacleCells) {
                bigObstacleCell.AddObstacle();
                obstacleCells.Add(bigObstacleCell);
            }

            Obstacle bigRock = Instantiate(obstaclePrefab, Grid.Instance.transform).GetComponent<Obstacle>();
            bigRock.CurrentType = Obstacle.Type.BigRock;
            bigRock.SetCoords(CellInfo.ToCoords(bigObstacleCells));
            obstacles.Add(bigRock);
        }

        // Generate hole
        List<CellInfo> holeCells = new List<CellInfo>(16);

        do {
            holeCells.Clear();
            CellInfo firstHoleCell = Grid.Instance.RetrieveRandomCell(3);

            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    holeCells.Add(Grid.Instance.CellInfoAt(firstHoleCell.Coords + new Vector2Int(i, j)));
                }
            }
        } while (!IsValidForObstacles(holeCells));

        foreach (var holeCell in holeCells) {
            holeCell.AddObstacle();
            obstacleCells.Add(holeCell);
        }

        Obstacle hole = Instantiate(obstaclePrefab, Grid.Instance.transform).GetComponent<Obstacle>();
        hole.CurrentType = Obstacle.Type.Hole;
        hole.SetCoords(CellInfo.ToCoords(holeCells));
        obstacles.Add(hole);
    }

    public void Reset() {
        foreach (var cell in obstacleCells) {
            cell.RemoveObstacle();
        }
        obstacleCells.Clear();

        foreach (var obstacle in obstacles) {
            Destroy(obstacle.gameObject);
        }
        obstacles.Clear();

        Generate();
    }

    private static bool IsValidForObstacles(CellInfo cell) {
        return IsValidForObstacles(new CellInfo[] { cell });
    }

    private static bool IsValidForObstacles(IEnumerable<CellInfo> cells) {
        foreach (var cell in cells) {
            if (cell.HasObstacle || cell.HasDamageHead || cell == Player.Instance.CurrentCell) {
                return false;
            }
        }

        return true;
    }
}
