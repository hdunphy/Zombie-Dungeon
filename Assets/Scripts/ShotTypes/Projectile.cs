using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float Damage;
    private Rigidbody2D RB;
    private GameObject Owner;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 velocity, float _damage, GameObject _owner)
    {
        Owner = _owner;
        Damage = _damage;
        RB.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != Owner)
        {
            if (collision.transform.TryGetComponent(out ITakeDamage entity))
            {
                entity.TakeDamage(Damage);
            }

            Destroy(gameObject);
        }
    }
}
