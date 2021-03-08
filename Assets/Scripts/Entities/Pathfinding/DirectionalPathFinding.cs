using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalPathFinding : MonoBehaviour, IPathFinding
{
    private Vector2 Target;

    public Vector2 GetDirection()
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        return (Target - position).normalized;
    }

    public void UpdatePath(Vector2 target)
    {
        Target = target;
    }
}
