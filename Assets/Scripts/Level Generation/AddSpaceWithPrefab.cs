using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpaceWithPrefab : MonoBehaviour, IAddSpaceToWorld
{

    [SerializeField] private GameObject Wall;
    [SerializeField] private GameObject Floor;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject SpanwerPrefab;

    public void AddSpaceToWorld(GridSpace gridSpace, Vector2 worldPosition)
    {
        GameObject prefab;
        switch (gridSpace)
        {
            case GridSpace.WALL:
                prefab = Wall;
                break;
            case GridSpace.FLOOR:
                prefab = Floor;
                break;
            case GridSpace.PLAYER:
                prefab = PlayerPrefab;
                break;
            case GridSpace.ENEMY:
                prefab = EnemyPrefab;
                break;
            case GridSpace.SPAWNER:
                prefab = SpanwerPrefab;
                break;
            default:
                Debug.LogWarning($"Not supposed to pass an {gridSpace} GridSpace");
                prefab = Floor;
                break;
        }

        Instantiate(prefab, worldPosition, Quaternion.identity);
    }
}
