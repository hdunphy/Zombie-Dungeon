using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private float AttackRange;
    [SerializeField] private float AttackDuration;
    [SerializeField] private float Damage;

    public void StartAttack()
    {
        Debug.Log($"{GetInstanceID()} Melee Attack");
        Vector2 attackPoint = transform.position + transform.up * AttackRange;
        var colliders = Physics2D.OverlapCircleAll(attackPoint, .5f);

        foreach (Collider2D hit in colliders)
        {
            if (hit.CompareTag("Player"))
            {
                AudioManager.Instance.PlaySound("Zombie Bite");
                hit.GetComponent<PlayerController>().TakeDamage(Damage);
                Debug.Log($"Does {Damage} Damage");
            }
        }
    }

    public float GetAttackRange()
    {
        return AttackRange;
    }

    public float GetAttackDuration()
    {
        return AttackDuration;
    }

    public bool CanAttack(Vector3 playerPosition)
    {
        return Vector3.Distance(transform.position, playerPosition) < 2 * AttackRange;
    }
}
