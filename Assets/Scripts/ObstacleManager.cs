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
    public GameObject towerPrefab;

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
                bigObstacleCells.Add(Grid.Instance.RetrieveRandomCell());

                List<CellInfo> adjacentCells = Grid.Instance.AdjacentTo(bigObstacleCells[0].Coords, false);
                bigObstacleCells.Add(adjacentCells[Random.Range(0, bigObstacleCells.Count)]);
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

        // Generate tower
        List<CellInfo> towerCells = new List<CellInfo>(4);

        do {
            towerCells.Add(Grid.Instance.RetrieveRandomCell(1));
            towerCells.Add(Grid.Instance.CellInfoAt(towerCells[0].Coords + new Vector2Int(1, 0)));
            towerCells.Add(Grid.Instance.CellInfoAt(towerCells[0].Coords + new Vector2Int(0, 1)));
            towerCells.Add(Grid.Instance.CellInfoAt(towerCells[0].Coords + new Vector2Int(1, 1)));
        } while (!IsValidForObstacles(towerCells));

        foreach (var towerCell in towerCells) {
            towerCell.AddObstacle();
            obstacleCells.Add(towerCell);
        }

        Obstacle tower = Instantiate(obstaclePrefab, Grid.Instance.transform).GetComponent<Obstacle>();
        tower.CurrentType = Obstacle.Type.Tower;
        tower.SetCoords(CellInfo.ToCoords(towerCells));
        obstacles.Add(tower);
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
