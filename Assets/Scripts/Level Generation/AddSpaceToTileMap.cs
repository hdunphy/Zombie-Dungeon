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

    private int GridToWorldConversion;

    public void AddSpaceToWorld(GridSpace gridSpace, Vector2 worldPosition)
    {
        //Vector3Int tilePosition;
        switch (gridSpace)
        {
            case GridSpace.WALL:
                //tilePosition = Wallmap.WorldToCell(worldPosition);
                //Floormap.SetTile(tilePosition, WallTile);
                AddTile(Wallmap, WallTile, worldPosition);
                break;
            case GridSpace.FLOOR:
                //tilePosition = Floormap.WorldToCell(worldPosition);
                //Floormap.SetTile(tilePosition, FloorTile);
                AddTile(Floormap, FloorTile, worldPosition);
                break;
            case GridSpace.PLAYER:
                Instantiate(PlayerPrefab, worldPosition, Quaternion.identity);
                break;
            case GridSpace.ENEMY:
                Instantiate(EnemyPrefab, worldPosition, Quaternion.identity);
                break;
            case GridSpace.SPAWNER:
                Instantiate(SpanwerPrefab, worldPosition, Quaternion.identity);
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
