﻿using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {
    public Vector2Int Coords { get; set; } = new Vector2Int(-1, -1);
    private readonly int smallCount = 2;
    private readonly int bigCount = 1;
    private List<CellInfo> obstacleCells = new List<CellInfo>();

    public GameObject smallRock;
    public GameObject bigRock;

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
            GameObject rock = Object.Instantiate(smallRock, Grid.Instance.transform);
            rock.transform.position = Grid.Instance.PositionForCoords(cell.Coords);
        }

        // Generate big obstacles
        for (int count = 0; count < bigCount; count++) {
            CellInfo[] bigObstacleCells = new CellInfo[2];

            do {
                bigObstacleCells[0] = Grid.Instance.RetrieveRandomCell();

                List<CellInfo> adjacentCells = Grid.Instance.AdjacentTo(bigObstacleCells[0].Coords, false);
                bigObstacleCells[1] = adjacentCells[Random.Range(0, bigObstacleCells.Length)];
            } while (!IsValidForObstacles(bigObstacleCells));

            foreach (var bigObstacleCell in bigObstacleCells) {
                bigObstacleCell.AddObstacle();
                obstacleCells.Add(bigObstacleCell);
            }
        }

        // Generate tower
        CellInfo[] towerCells = new CellInfo[4];

        do {
            towerCells[0] = Grid.Instance.RetrieveRandomCell(1);
            towerCells[1] = Grid.Instance.CellInfoAt(towerCells[0].Coords + new Vector2Int(1, 0));
            towerCells[2] = Grid.Instance.CellInfoAt(towerCells[0].Coords + new Vector2Int(0, 1));
            towerCells[3] = Grid.Instance.CellInfoAt(towerCells[0].Coords + new Vector2Int(1, 1));
        } while (!IsValidForObstacles(towerCells));

        foreach (var towerCell in towerCells) {
            towerCell.AddObstacle();
            obstacleCells.Add(towerCell);
        }
    }

    public void Reset() {
        foreach (var cell in obstacleCells) {
            cell.RemoveObstacle();
        }
        obstacleCells.Clear();

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
