using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour, IPathFinding
{
    [SerializeField] private LayerMask WallMask;
    [SerializeField] private float FollowRadius;
    public bool _debug;

    private Stack<Vector2> Path;
    private Vector2 nextMove;
    private Vector2Int target;

    private void Start()
    {
        Path = new Stack<Vector2>();
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.black;
        if (_debug && Application.isPlaying)
        {
            foreach (Vector2 _tile in Path)
            {
                Gizmos.DrawSphere(_tile, .2f);
            }
        }
    }

    public Vector2 GetDirection()
    {
        Vector2 direction;
        Vector2 pos = transform.position;

        if ((pos - target).sqrMagnitude < (FollowRadius))
        {
            direction = (target - pos).normalized;
        }
        else
        {
            if ((nextMove - pos).sqrMagnitude < 0.01)
            {
                nextMove = Path.Pop();
            }
            direction = (nextMove - pos).normalized;
        }

        return direction;
    }

    public void UpdatePath(Vector2 _target)
    {
        Vector2Int roundedTarget = Vector2Int.RoundToInt(_target);
        if (roundedTarget != target)
        {
            target = roundedTarget;
            CalculatePath();
        }
    }

    private void CalculatePath()
    {
        Path.Clear();
        Vector2Int currentPos = Vector2Int.RoundToInt(transform.position);

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, int> costSoFar = new Dictionary<Vector2Int, int>
        {
            { currentPos, 0 }
        };

        PriorityQueue<Vector2Int> checkTileQueue = new PriorityQueue<Vector2Int>();
        checkTileQueue.Add(new PriorityElement<Vector2Int>(currentPos, GetHueristic(currentPos)));


        while (!checkTileQueue.IsEmpty())
        {
            PriorityElement<Vector2Int> _CurrentElement = checkTileQueue.Dequeue();
            if (_CurrentElement.Item.Equals(target))
            {
                break;
            }

            List<Vector2Int> possibleMoves = GetAdjacents(_CurrentElement.Item);
            foreach (Vector2Int _move in possibleMoves)
            {
                int cost = costSoFar[_CurrentElement.Item] + 1;
                if (!costSoFar.ContainsKey(_move))
                {
                    costSoFar.Add(_move, cost);
                    AddPossibleMove(cost, _move, checkTileQueue, cameFrom, _CurrentElement.Item);
                }
                else if (cost < costSoFar[_move])
                {
                    costSoFar[_move] = cost;
                    AddPossibleMove(cost, _move, checkTileQueue, cameFrom, _CurrentElement.Item);
                }
            }
        }

        if (cameFrom.ContainsKey(target))
        {
            GeneratePath(cameFrom, currentPos);
        }
    }

    private void GeneratePath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int currentPos)
    {
        Vector2Int _nextMove = target;
        while (!_nextMove.Equals(currentPos))
        {

            Path.Push(_nextMove);
            _nextMove = cameFrom[_nextMove];
        }

        nextMove = transform.position;
    }

    private void AddPossibleMove(int cost, Vector2Int _move, PriorityQueue<Vector2Int> checkTileQueue,
        Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int _currentTile)
    {
        int prority = cost + GetHueristic(_move);
        checkTileQueue.Add(new PriorityElement<Vector2Int>(_move, prority));
        if (cameFrom.ContainsKey(_move))
        {
            cameFrom[_move] = _currentTile;
        }
        else
        {
            cameFrom.Add(_move, _currentTile);
        }
    }

    private List<Vector2Int> GetAdjacents(Vector2Int currentTile)
    {
        List<Vector2Int> adjacents = new List<Vector2Int>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector2Int _tile = currentTile + new Vector2Int(i, j);

                if (!(i == 0 && j == 0))
                {
                    if (!Physics2D.OverlapCircle(_tile, .2f, WallMask))
                    {
                        adjacents.Add(_tile);
                    }
                }
            }
        }

        return adjacents;
    }

    private int GetHueristic(Vector2Int currentPos)
    {
        return Mathf.RoundToInt(Vector2Int.Distance(currentPos, target));
    }
}

public class PriorityElement<T>
{
    public T Item { get; private set; }
    public int Priority { get; private set; }
    public PriorityElement(T _item, int _priority)
    {
        Item = _item;
        Priority = _priority;
    }

    public void SetPriority(int priority)
    {
        Priority = priority;
    }
}

public class PriorityQueue<T>
{
    private List<PriorityElement<T>> Queue;

    public PriorityQueue()
    {
        Queue = new List<PriorityElement<T>>();
    }

    public bool IsEmpty() { return Queue.Count == 0; }

    public void Add(PriorityElement<T> element)
    {
        int index;
        for (index = 0; index < Queue.Count; index++)
        {
            if (element.Priority < Queue[index].Priority)
            {
                break;
            }
        }

        Queue.Insert(index, element);
    }

    ////Returns true if replaced an element's priority
    ////False if no element priority was replaced
    //public bool UpdatePriority(PriorityElement<T> element)
    //{
    //    bool changed = false;

    //    PriorityElement<T> existingElement = Queue.Find(x => x.Item.Equals(element.Item));

    //    if (existingElement != null && existingElement.Priority > element.Priority)
    //    {
    //        changed = true;
    //        existingElement.SetPriority(element.Priority);
    //    }

    //    return changed;
    //}

    public PriorityElement<T> Dequeue()
    {
        PriorityElement<T> head;

        if (Queue.Count == 0)
        {
            head = null;
        }
        else
        {
            head = Queue[0];
            Queue.RemoveAt(0);
        }

        return head;
    }

    public PriorityElement<T> Peek()
    {
        return Queue[0];
    }
}
