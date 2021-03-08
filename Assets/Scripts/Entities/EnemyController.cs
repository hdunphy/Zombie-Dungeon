using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IPathFinding))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    [SerializeField] private Movement Movement;
    [SerializeField] private float Health;
    [SerializeField] private float AttackRange;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private float Damage;
    [Range(0, 1)]
    [SerializeField] private float AmmoDropChance;
    [SerializeField] private GameObject AmmoDrop;
    [Range(0, 1)]
    [SerializeField] private float WeaponDropChance;
    [SerializeField] private GameObject WeaponDrop;

    private PlayerController Player;
    private IPathFinding PathFinding;
    private bool isAttacking = false;

    private void Start()
    {
        PathFinding = GetComponent<IPathFinding>();
        Player = FindObjectOfType<PlayerController>();
    }

    private void FixedUpdate()
    {
        bool isMoving = false;
        if (Player != null && !isAttacking)
        {
            if (Vector3.Distance(transform.position, Player.transform.position) > 2 * AttackRange)
            {
                PathFinding.UpdatePath(Player.transform.position);
                var Dir = PathFinding.GetDirection();
                Vector2Int dirInt = Vector2Int.RoundToInt(Dir);
                if (dirInt != Vector2.zero)
                {
                    isMoving = true;
                }
                Movement.SetMoveDirection(Dir);
            }
            else
            {
                isAttacking = true;
                Animator.SetTrigger("IsAttacking");
                //StartCoroutine(Attack());
            }
        }
        else
            Movement.SetMoveDirection(Vector2.zero);
        Animator.SetBool("IsMoving", isMoving);
    }

    //Gets called by Animator
    private void Attack()
    {
        //yield return new WaitForSeconds(AttackSpeed);

        Vector2 attackPoint = transform.position + transform.up * AttackRange;
        var colliders = Physics2D.OverlapCircleAll(attackPoint, .5f);
        //Debug.Log($"Attack Point: {attackPoint}");

        foreach (Collider2D hit in colliders)
        {
            if (hit.CompareTag("Player"))
            {
                Player.TakeDamage(Damage);
            }
        }

        StartCoroutine(WaitForNextAttack());
    }

    private IEnumerator WaitForNextAttack()
    {
        yield return new WaitForSeconds(AttackSpeed);

        isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            if (UnityEngine.Random.value < AmmoDropChance)
            {
                Instantiate(AmmoDrop, transform.position, Quaternion.identity);
            }
            else if (UnityEngine.Random.value < WeaponDropChance)
            {
                Instantiate(WeaponDrop, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        var point = transform.position + transform.up * AttackRange;

        Gizmos.DrawSphere(point, .5f);
    }
}
