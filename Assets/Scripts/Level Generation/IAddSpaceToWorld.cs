using UnityEngine;

public enum GridSpace { EMPTY, FLOOR, WALL, PLAYER, ENEMY, SPAWNER, DEVIL }

public interface IAddSpaceToWorld
{
    void AddSpaceToWorld(GridSpace gridSpace, Vector2 worldPosition);
    void SetUpComplete();
    void InitializeGridConversion(int GridToWorldConversion);
}
