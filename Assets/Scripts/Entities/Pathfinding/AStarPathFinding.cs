using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour, IPathFinding
{
    [SerializeField] private LayerMask WallMask;
    [SerializeField] private float FollowRadius;
    [SerializeField] private float StepSize;
    [SerializeField] private float DistanceCheck;

    public bool _debug;

    private Stack<Vector2> Path;
    private Vector2 nextMove;
    private Vector2Int target;

    private void Awake()
    {
        Path = new Stack<Vector2>();
        nextMove = transform.position;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.black;
        if (_debug && Application.isPlaying)
        {
            foreach (Vector2 _tile in Path)
            {
                Gizmos.DrawSphere(_tile, .1f);
            }
        }
    }

    public Vector2 GetDirection()
    {
        Vector2 direction;
        Vector2 pos = transform.position;

        if ((pos - target).sqrMagnitude < (FollowRadius * FollowRadius) || Path.Count == 0)
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
        float Distance = Vector2.Distance(_target, transform.position);
        if (roundedTarget != target && Distance < DistanceCheck)
        {
            target = roundedTarget;
            CalculatePath();
        }
    }

    private void CalculatePath()
    {
        Path.Clear();
        Vector2 currentPos = Vector2Int.RoundToInt(transform.position);

        Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();
        Dictionary<Vector2, float> costSoFar = new Dictionary<Vector2, float>
        {
            { currentPos, 0 }
        };

        PriorityQueue<Vector2> checkTileQueue = new PriorityQueue<Vector2>();
        checkTileQueue.Add(new PriorityElement<Vector2>(currentPos, GetHueristic(currentPos)));


        while (!checkTileQueue.IsEmpty())
        {
            PriorityElement<Vector2> _CurrentElement = checkTileQueue.Dequeue();
            if (_CurrentElement.Item.Equals(target))
            {
                break;
            }

            List<Vector2> possibleMoves = GetAdjacents(_CurrentElement.Item);
            foreach (Vector2 _move in possibleMoves)
            {
                float cost = costSoFar[_CurrentElement.Item] + StepSize;
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

    private void GeneratePath(Dictionary<Vector2, Vector2> cameFrom, Vector2 currentPos)
    {
        Vector2 _nextMove = target;
        while (!_nextMove.Equals(currentPos))
        {

            Path.Push(_nextMove);
            _nextMove = cameFrom[_nextMove];
        }

        nextMove = transform.position;
    }

    private void AddPossibleMove(float cost, Vector2 _move, PriorityQueue<Vector2> checkTileQueue,
        Dictionary<Vector2, Vector2> cameFrom, Vector2 _currentTile)
    {
        float prority = cost + GetHueristic(_move);
        checkTileQueue.Add(new PriorityElement<Vector2>(_move, prority));
        if (cameFrom.ContainsKey(_move))
        {
            cameFrom[_move] = _currentTile;
        }
        else
        {
            cameFrom.Add(_move, _currentTile);
        }
    }

    private List<Vector2> GetAdjacents(Vector2 currentTile)
    {
        List<Vector2> adjacents = new List<Vector2>();
        for (float i = -StepSize; i <= StepSize; i += StepSize)
        {
            for (float j = -StepSize; j <= StepSize; j += StepSize)
            {
                Vector2 _tile = currentTile + new Vector2(i, j);

                if (!(i == 0 && j == 0))
                {
                    if (!Physics2D.OverlapCircle(_tile, .495f, WallMask))
                    {
                        adjacents.Add(_tile);
                    }
                }
            }
        }

        return adjacents;
    }

    private float GetHueristic(Vector2 currentPos)
    {
        return Vector2.Distance(currentPos, target);
    }
}

public class PriorityElement<T>
{
    public T Item { get; private set; }
    public float Priority { get; private set; }
    public PriorityElement(T _item, float _priority)
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
