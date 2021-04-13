using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AddSpaceToTileMap : MonoBehaviour, IAddSpaceToWorld
{
    public Tilemap Floormap;
    public Tilemap Wallmap;
    public TileBase FloorTile;
    public TileBase WallTile;

    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject SpanwerPrefab;
    [SerializeField] private GameObject DevilPrefab;
    [SerializeField] private Vector2 TilemapOffset;

    private int GridToWorldConversion;

    public void AddSpaceToWorld(GridSpace gridSpace, Vector2 worldPosition)
    {
        switch (gridSpace)
        {
            case GridSpace.WALL:
                AddTile(Wallmap, WallTile, worldPosition);
                break;
            case GridSpace.FLOOR:
                AddTile(Floormap, FloorTile, worldPosition);
                break;
            case GridSpace.PLAYER:
                Instantiate(PlayerPrefab, worldPosition + TilemapOffset, Quaternion.identity);
                break;
            case GridSpace.ENEMY:
                Instantiate(EnemyPrefab, worldPosition + TilemapOffset, Quaternion.identity);
                break;
            case GridSpace.SPAWNER:
                Instantiate(SpanwerPrefab, worldPosition + TilemapOffset, Quaternion.identity);
                break;
            case GridSpace.DEVIL:
                Instantiate(DevilPrefab, worldPosition + TilemapOffset, Quaternion.identity);
                break;
            default:
                Debug.LogWarning($"Not supposed to pass an {gridSpace} GridSpace");
                break;
        }

    }

    private void AddTile(Tilemap map, TileBase tile, Vector2 worldPosition)
    {
        for(int i = 0; i < GridToWorldConversion; i++)
        {
            for(int j = 0; j < GridToWorldConversion; j++)
            {
                Vector3 _pos = new Vector3(worldPosition.x + i, worldPosition.y + j);
                map.SetTile(map.WorldToCell(_pos), tile);
            }
        }
    }

    public void InitializeGridConversion(int _GridToWorldConversion)
    {
        GridToWorldConversion = _GridToWorldConversion;
    }

    public void SetUpComplete()
    {
        //Wallmap.GetComponent<CompositeCollider2D>().GenerateGeometry();
    }
}
