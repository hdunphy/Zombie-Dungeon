using UnityEngine;

public enum GridSpace { EMPTY, FLOOR, WALL, PLAYER, ENEMY }

public interface IAddSpaceToWorld
{
    void AddSpaceToWorld(GridSpace gridSpace, Vector2 worldPosition);
}
