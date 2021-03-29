using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour, IPathFinding
{
    private Queue<Vector2> Path;
    private Vector2 nextMove;

    public Vector2 GetDirection()
    {
        throw new System.NotImplementedException();
    }

    public void UpdatePath(Vector2 target)
    {
        throw new System.NotImplementedException();
    }
}
