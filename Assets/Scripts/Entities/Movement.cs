using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D Rb;
    [SerializeField] private Transform SpriteTransform;
    [SerializeField] private float MoveSpeed;

    private readonly Dictionary<Vector2, float> RotationValues = new Dictionary<Vector2, float>
        {
            { Vector2.right, 270 },
            { Vector2.right + Vector2.up, 315 },
            { Vector2.up, 0 },
            { Vector2.left + Vector2.up, 45 },
            { Vector2.left, 90 },
            { Vector2.left + Vector2.down, 135 },
            { Vector2.down, 180 },
            { Vector2.right + Vector2.down, 225 }
        };
    private Vector2 moveDirection;
    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            Rb.velocity = (moveDirection * MoveSpeed * Time.deltaTime);
        }
        else
        {
            Rb.velocity = Vector2.zero;
        }
    }

    public void SetMoveDirection(Vector2 _moveDirection)
    {
        moveDirection = _moveDirection;
        var dir = Vector2Int.RoundToInt(moveDirection);
        if (dir != Vector2.zero)
        {
            if(!RotationValues.TryGetValue(dir, out float z))
                Debug.LogWarning($"No value for {dir}");
            SpriteTransform.eulerAngles = new Vector3(0, 0, z);
        }
    }

    public void SetCanMove(bool _canMove)
    {
        canMove = _canMove;
    }
}
