using UnityEngine;

public class ObstacleAvoidancePathFinding : MonoBehaviour, IPathFinding
{
    [SerializeField] private LayerMask WallLayer;

    private Vector2 Target;
    private PathFindingState State;
    private Vector3 ObstacleDirection;

    //private Vector2 obstacle

    private void Start()
    {
        State = PathFindingState.Hunting;
    }

    public Vector2 GetDirection()
    {
        GetState();

        Vector2 direction;
        switch (State)
        {
            case PathFindingState.Hunting:
                direction = GetHuntingStateDirection();
                break;
            case PathFindingState.Stuck:
                direction = GetStuckStateDirection();
                break;
            //case PathFindingState.Idle:
            default:
                direction = Vector2.zero;
                break;
        }

        return direction.normalized;
    }

    public void UpdatePath(Vector2 target)
    {
        Target = target;
    }

    private void GetState()
    {
        if(Physics2D.Raycast(transform.position, transform.right, 1f, WallLayer))
        {
            State = PathFindingState.Stuck;
            ObstacleDirection = transform.right;
        }
        else if(PathFindingState.Stuck == State && Physics2D.Raycast(transform.position, ObstacleDirection, 1f, WallLayer))
        {
            State = PathFindingState.Hunting;
        }
    }

    private Vector2 GetStuckStateDirection()
    {
        Vector2 finalDir;
        Vector2 pos = transform.position;
        Vector2 directionA = transform.right * new Vector2(-1, 1);
        Vector2 directionB = transform.right * new Vector2(1, -1);

        bool canGoA = !Physics2D.Raycast(pos, directionA, 1f, WallLayer);
        bool canGoB = !Physics2D.Raycast(pos, directionB, 1f, WallLayer);

        if(canGoA && canGoB)
        {
            finalDir = Vector2.Distance(pos + directionA, Target) > Vector2.Distance(pos + directionB, Target) ?
                directionB : directionA;
        }
        else if(canGoA || canGoB)
        {
            finalDir = canGoA ? directionA : directionB;
        }
        else
        {
            finalDir = transform.right * new Vector2(-1, -1);
        }
        return finalDir;
    }

    private Vector2 GetHuntingStateDirection()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        return (Target - position);
    }
}
