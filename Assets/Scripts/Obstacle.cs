using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {
    public enum Type {
        None,
        SmallRock,
        BigRock,
        Tower
    }

    public Type currentType = Type.None;
    public Type CurrentType {
        get {
            return currentType;
        }

        set {
            currentType = value;
            SyncDisplayObject();
        }
    }

    public Vector2Int SWCoords { get; set; } = new Vector2Int(-1, -1);
    public Vector2Int NECoords { get; set; } = new Vector2Int(-1, -1);

    private GameObject displayObject;

    public void SetCoords(Vector2Int coords) {
        SetCoords(new List<Vector2Int> { coords });
    }

    public void SetCoords(List<Vector2Int> coordsList) {
        int minX = int.MaxValue;
        int maxX = -1;
        int minY = int.MaxValue;
        int maxY = -1;

        foreach (var coords in coordsList) {
            if (coords.x < minX) {
                minX = coords.x;
            }

            if (coords.x > maxX) {
                maxX = coords.x;
            }

            if (coords.y < minY) {
                minY = coords.y;
            }

            if (coords.y > maxY) {
                maxY = coords.y;
            }
        }

        SWCoords = new Vector2Int(minX, minY);
        NECoords = new Vector2Int(maxX, maxY);

        Vector3 SWPosition = Grid.Instance.PositionForCoords(SWCoords);
        Vector3 NEPosition = Grid.Instance.PositionForCoords(NECoords);
        Vector3 centerPosition = (SWPosition + NEPosition) * 0.5f;
        transform.position = new Vector3(
            centerPosition.x,
            transform.position.y,
            centerPosition.z
        );

        int width = NECoords.x - SWCoords.x;
        int height = NECoords.y - SWCoords.y;

        if (width > height) {
            // It's a wide object, rotate 90 degrees.
            transform.localEulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
        } else {
            // It's a tall object, no rotation since our model default assumes tall.
            transform.localEulerAngles = Vector3.zero;
        }
    }

    private void SyncDisplayObject() {
        if (displayObject != null) {
            Destroy(displayObject);
        }

        GameObject prefab = null;

        switch (CurrentType) {
            case Type.SmallRock:
                prefab = ObstacleManager.Instance.smallRockPrefab;
                break;
            case Type.BigRock:
                prefab = ObstacleManager.Instance.bigRockPrefab;
                break;
            case Type.Tower:
                prefab = ObstacleManager.Instance.towerPrefab;
                break;
        }

        if (prefab != null) {
            displayObject = Instantiate(prefab, transform);
        }
    }
}
