using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpaceWithPrefab : MonoBehaviour, IAddSpaceToWorld
{

    [SerializeField] private GameObject Wall;
    [SerializeField] private GameObject Floor;
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject EnemyPrefab;

    public void AddSpaceToWorld(GridSpace gridSpace, Vector2 worldPosition)
    {
        GameObject space;
        switch (gridSpace)
        {
            case GridSpace.WALL:
                space = Instantiate(Wall, worldPosition, Quaternion.identity);
                break;
            case GridSpace.FLOOR:
                space = Instantiate(Floor, worldPosition, Quaternion.identity);
                break;
            case GridSpace.PLAYER:
                space = Instantiate(PlayerPrefab, worldPosition, Quaternion.identity);
                break;
            case GridSpace.ENEMY:
                space = Instantiate(EnemyPrefab, worldPosition, Quaternion.identity);
                break;
            default:
                Debug.LogWarning($"Not supposed to pass an {gridSpace} GridSpace");
                break;
        }
    }
}
