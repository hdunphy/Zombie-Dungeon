using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Size")]
    [SerializeField] private int Width;
    [SerializeField] private int Height;
    [SerializeField] private int MaxWalkers;
    [SerializeField] private int GridToWorldConversion;
    [Range(0, 1)]
    [SerializeField] private float PercentToFill;

    [Header("Random Values")]
    [Range(0, 1)]
    [SerializeField] private float SpawnWalkerChance;
    [Range(0, 1)]
    [SerializeField] private float DestroyWalkerChance;
    [Range(0, 1)]
    [SerializeField] private float WalkerChangeDirectionChance;

    [Header("Entities")]
    [SerializeField] private int EnemyStartNumber;
    
    [Header("Debug")]
    [SerializeField] private bool ShowLevelCreation;

    struct Walker
    {
        public Vector2Int Direction;
        public Vector2 Position;
    }

    private List<LevelPrefab> LevelPrefabs;
    private IAddSpaceToWorld AddSpaceImplementation;
    private List<Walker> Walkers;
    private GridSpace[,] LevelGrid;
    private int FloorCount;

    private readonly List<Vector2Int> Directions = new List<Vector2Int>() { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    public GridSpace[,] GetLevelGrid() { return LevelGrid; }

    public void StartLevelCreation()
    {
        SetUp();
        StartWalkers();
        PostFloorCreationJobs();
    }

    private void SetUpPrefabs(int enemyStartNumber, List<LevelPrefab> _levelPrefabs)
    {
        EnemyStartNumber = enemyStartNumber;

        LevelPrefabs = _levelPrefabs;
    }

    public void StartLevelCreation(int enemyStartNumber, List<LevelPrefab> _levelPrefabs)
    {
        SetUpPrefabs(enemyStartNumber, _levelPrefabs);

        StartLevelCreation();
    }

    public void NextLevel(int enemyStartNumber, Vector2 playerWorldSpace)
    {
        EnemyStartNumber = enemyStartNumber;

        ResetNonFloorWallTiles();

        //Update Player Position
        Vector2Int playerGridSpace = WalkerPositionToGridPosition(playerWorldSpace);
        LevelGrid[playerGridSpace.x, playerGridSpace.y] = GridSpace.PLAYER;


        AddEnemies();
        AddLevelPrefabs();
    }

    private void ResetNonFloorWallTiles()
    {
        for(int i = 0; i < Width; i++)
        {
            for(int j = 0; j < Height; j++)
            {
                GridSpace gridSpace = LevelGrid[i, j];
                if (gridSpace != GridSpace.EMPTY && gridSpace != GridSpace.FLOOR && gridSpace != GridSpace.WALL)
                {
                    LevelGrid[i, j] = GridSpace.FLOOR;
                }
            }
        }
    }

    private void PostFloorCreationJobs()
    {
        AddWalls();
        AddPlayer();
        AddEnemies();
        AddLevelPrefabs();
    }

    private void SetUp()
    {
        AddSpaceImplementation = GetComponent<IAddSpaceToWorld>();
        AddSpaceImplementation.InitializeGridConversion(GridToWorldConversion);

        FloorCount = 0;
        LevelGrid = new GridSpace[Width, Height];
        Vector2Int middle = Vector2Int.RoundToInt(new Vector2(Width / 2f, Height / 2f));
        Walkers = new List<Walker>() { new Walker { Direction = GetRandomDirection(), Position = GridPositionToWalkerPosition(middle) } };

        AddSpaceToGrid(GridSpace.FLOOR, middle);
    }

    private void IterateWalkers()
    {
        //Check to destroy
        for (int i = 1; i < Walkers.Count; i++)
        {
            if (RollRandomChance(DestroyWalkerChance))
            {
                Walkers.RemoveAt(i);
                break;
            }
        }

        //Update Position and add Floor
        for (int i = 0; i < Walkers.Count; i++)
        {
            Walker _walker = Walkers[i];
            _walker.Position += GridPositionToWalkerPosition(_walker.Direction);
            Walkers[i] = _walker;

            Vector2Int gridPosition = WalkerPositionToGridPosition(_walker.Position);

            //FOR DEBUG
            if (!IsVectorInGrid_Floor(gridPosition))
                Debug.Log($"Walker dir: {_walker.Direction}, Walker pos {_walker.Position}");

            if (IsVectorInGrid_Floor(gridPosition) && LevelGrid[gridPosition.x, gridPosition.y] != GridSpace.FLOOR)
            {
                AddSpaceToGrid(GridSpace.FLOOR, gridPosition);
            }

            //Check for a child walker spawn
            if (Walkers.Count < MaxWalkers && RollRandomChance(SpawnWalkerChance))
            {
                Walker _child = new Walker { Position = _walker.Position };
                _child.Direction = UpdateWalkerDirection(_child);
                if (_child.Direction != Vector2Int.zero)
                    Walkers.Add(_child);
            }
        }

        //Find new valid direction
        for (int i = 0; i < Walkers.Count; i++)
        {
            Walker _walker = Walkers[i];
            Vector2Int nextPosition = GetNextPosition(_walker);
            if (!IsVectorInGrid_Floor(nextPosition) || RollRandomChance(WalkerChangeDirectionChance))
            {
                _walker.Direction = UpdateWalkerDirection(Walkers[i]);
                Walkers[i] = _walker;
            }
        }
    }

    private void StartWalkers()
    {
        int iterations = 0;

        while (Walkers.Count > 0)
        {
            if (iterations++ > 100)
            {
                Debug.Log("Too many iterations");
                break;
            }

            IterateWalkers();

            float currentFloorPercent = (float)FloorCount / (Width * Height);
            if (currentFloorPercent >= PercentToFill)
            {
                //End of Level Generation
                Walkers.Clear();
                break;
            }
        }
    }

    private IEnumerator StartWalkersCoroutine()
    {
        while (Walkers.Count > 0)
        {
            IterateWalkers();

            float currentFloorPercent = (float)FloorCount / (Width * Height);
            if (currentFloorPercent >= PercentToFill)
            {
                //End of Level Generation
                Walkers.Clear();
                break;
            }

            yield return new WaitForSeconds(0.3f);
        }

        PostFloorCreationJobs();
    }

    private void AddWalls()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                GridSpace _space = LevelGrid[i, j];
                if (_space == GridSpace.EMPTY)
                {
                    CheckToAddWall(i, j);
                }
            }
        }

        StartCoroutine(FinishedBuildingLevel());
    }

    private IEnumerator FinishedBuildingLevel()
    {
        yield return new WaitForEndOfFrame();

        AddSpaceImplementation.SetUpComplete();
    }

    private void AddPlayer()
    {
        AddSpaceToGrid(GridSpace.PLAYER, GetRandomOpenFloorSpace());
    }

    private void AddEnemies()
    {
        for (int i = 0; i < EnemyStartNumber; i++)
        {
            AddSpaceToGrid(GridSpace.ENEMY, GetRandomOpenFloorSpace());
        }
    }

    private void AddLevelPrefabs()
    {
        foreach (LevelPrefab _levelPrefab in LevelPrefabs)
        {
            for (int i = 0; i < _levelPrefab.Instances; i++)
                AddSpaceToGrid(_levelPrefab.SpaceType, GetRandomOpenFloorSpace(_levelPrefab.RequiredSpace, 0));
        }
    }

    private void CheckToAddWall(int i, int j)
    {
        Vector2Int _position = new Vector2Int(i, j);
        bool addWall = false;
        int numberOfAdjacentFloors = 0;

        foreach (Vector2Int dir in Directions)
        {
            Vector2Int adjacentPos = _position + dir;
            if (IsVectorInGrid(adjacentPos) && LevelGrid[adjacentPos.x, adjacentPos.y] == GridSpace.FLOOR)
            {
                addWall = true;
                numberOfAdjacentFloors++;
            }
        }

        if (numberOfAdjacentFloors > 3)
        {
            AddSpaceToGrid(GridSpace.FLOOR, _position);
        }
        else if (addWall)
        {
            AddSpaceToGrid(GridSpace.WALL, _position);
        }
    }

    private void AddSpaceToGrid(GridSpace gridSpace, Vector2Int gridPosition)
    {
        LevelGrid[gridPosition.x, gridPosition.y] = gridSpace;
        Vector2 _position = GridPositionToWalkerPosition(gridPosition);

        if (GridSpace.FLOOR == gridSpace)
            FloorCount++;
        AddSpaceImplementation.AddSpaceToWorld(gridSpace, _position);
    }

    private Vector2Int UpdateWalkerDirection(Walker walker, int timesCalled = 0)
    {
        //Don't want stack overflow
        if (timesCalled > 6)
            return Vector2Int.zero;

        Vector2Int dir = GetRandomDirection();
        walker.Direction = dir;
        if (!IsVectorInGrid_Floor(GetNextPosition(walker)))
        {
            dir = UpdateWalkerDirection(walker, timesCalled + 1);
        }

        return dir;
    }

    private Vector2Int GetRandomOpenFloorSpace(int count = 0)
    {
        if (count > 10)
        {
            return Vector2Int.RoundToInt(new Vector2(Width / 2f, Height / 2f));
        }

        Vector2Int space = GetRandomPositionInGrid();
        if (LevelGrid[space.x, space.y] != GridSpace.FLOOR)
        {
            space = GetRandomOpenFloorSpace(count + 1);
        }
        return space;
    }

    private Vector2Int GetRandomOpenFloorSpace(int gridSpaceNeeded, int count = 0)
    {
        if (count > 10)
        {
            return Vector2Int.RoundToInt(new Vector2(Width / 2f, Height / 2f));
        }

        Vector2Int space = GetRandomPositionInGrid();

        bool containsNonFloor = false;
        for (int i = space.x - gridSpaceNeeded; i <= space.x + gridSpaceNeeded; i++)
        {
            for (int j = space.y - gridSpaceNeeded; j <= space.y + gridSpaceNeeded; j++)
            {
                Vector2Int _adjSpace = new Vector2Int(i, j);
                if (!IsVectorInGrid_Floor(_adjSpace) || LevelGrid[i, j] != GridSpace.FLOOR)
                {
                    containsNonFloor = true;
                    break;
                }
            }
            if (containsNonFloor)
                break;
        }
        if (containsNonFloor)
        {
            space = GetRandomOpenFloorSpace(gridSpaceNeeded, count + 1);
        }
        return space;
    }

    #region Conversions
    private Vector2Int WalkerPositionToGridPosition(Vector2 walkerPosition)
    {
        return Vector2Int.RoundToInt(new Vector2(walkerPosition.x / GridToWorldConversion, walkerPosition.y / GridToWorldConversion));
    }

    private Vector2 GridPositionToWalkerPosition(Vector2Int gridPosition)
    {
        return gridPosition * GridToWorldConversion;
    }

    private Vector2Int GetNextPosition(Walker walker)
    {
        return WalkerPositionToGridPosition(walker.Position + (GridToWorldConversion * walker.Direction));
    }

    private bool IsVectorInGrid_Floor(Vector2Int pos)
    {
        return pos.x >= 1 && pos.x < Width - 1 && pos.y >= 1 && pos.y < Height - 1;
    }

    private bool IsVectorInGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < Width && pos.y >= 0 && pos.y < Height;
    }
    #endregion

    #region Random Cacluations

    private Vector2Int GetRandomPositionInGrid()
    {
        int x = UnityEngine.Random.Range(0, Width);
        int y = UnityEngine.Random.Range(0, Height);
        return new Vector2Int(x, y);
    }
    private bool RollRandomChance(float chance)
    {
        float roll = UnityEngine.Random.value;
        return chance > roll;
    }

    private Vector2Int GetRandomDirection()
    {
        return Directions[UnityEngine.Random.Range(0, Directions.Count)];
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            foreach (Walker _walker in Walkers)
            {
                Gizmos.DrawSphere(new Vector3(_walker.Position.x, _walker.Position.y), GridToWorldConversion / 2f);
            }
        }

    }
}

[Serializable]
public class LevelPrefab
{
    public float StartNumber;
    public float LevelIncrease;
    public int RequiredSpace;
    public GridSpace SpaceType;
    public int Instances { get; private set; }
    public float CurrentInstances { get; private set; }

    public void Start()
    {
        Instances = Mathf.FloorToInt(StartNumber);
        CurrentInstances = StartNumber;
    }

    public void NextLevel()
    {
        CurrentInstances *= (1 + LevelIncrease);
        Instances = Mathf.FloorToInt(CurrentInstances);
    }
}
