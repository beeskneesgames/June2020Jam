using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageHead {
    public static bool diagonalAllowed = false;
    public static bool preferHealthyCells = true;

    public Vector2Int coords;
    public CellInfo CurrentCellInfo {
        get {
            return Grid.Instance.CellInfoAt(coords);
        }
    }

    public Vector2Int Coords {
        get {
            return coords;
        }
        set {
            coords = value;
            CurrentCellInfo.Damage();
            gameObject.transform.position = Grid.Instance.PositionForCoords(Coords);
            propertyBlock.SetVector("_Seed", new Vector4(Coords.x, Coords.y));
            renderer.SetPropertyBlock(propertyBlock);
        }
    }

    private GameObject gameObject;
    private DamageHeadController controller;
    private Renderer renderer;
    private MaterialPropertyBlock propertyBlock;

    public DamageHead(Vector2Int coords, GameObject damageHeadGameObject) {
        gameObject = damageHeadGameObject;
        controller = damageHeadGameObject.GetComponent<DamageHeadController>();
        renderer = gameObject.GetComponent<DamageHeadController>().model.GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();

        Coords = coords;
    }

    public void Move() {
        List<CellInfo> possibleCells = new List<CellInfo>();

        foreach (var cell in Grid.Instance.AdjacentTo(coords, diagonalAllowed)) {
            if (!cell.HasObstacle && !cell.HasPlayer) {
                possibleCells.Add(cell);
            }
        }

        if (preferHealthyCells) {
            List<CellInfo> healthyCells = new List<CellInfo>();

            foreach (var cell in possibleCells) {
                if (cell.IsHealthy) {
                    healthyCells.Add(cell);
                }
            }

            if (healthyCells.Count > 0) {
                possibleCells = healthyCells;
            }
        }

        if (possibleCells.Count > 0) {
            Vector2Int nextCoords = possibleCells[UnityEngine.Random.Range(0, possibleCells.Count)].Coords;

            // If negative, the nextCoords is still set as its sentinel value,
            // which is effectively null here.
            if (nextCoords.x >= 0) {
                Coords = nextCoords;
            }
        }
    }

    public void DestroyGameObject() {
        controller.DestroyGameObject();
    }
}
