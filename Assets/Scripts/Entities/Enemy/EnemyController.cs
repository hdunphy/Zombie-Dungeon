using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IPathFinding))]
public class EnemyController : MonoBehaviour, ITakeDamage
{
    [SerializeField] private Animator Animator;
    [SerializeField] private Movement Movement;
    [SerializeField] private float Health;
    //[SerializeField] private float AttackRange;
    //[SerializeField] private float AttackSpeed;
    //[SerializeField] private float Damage;
    [SerializeField] private EntityDropTable EntityDropTable;

    private PlayerController Player;
    private IPathFinding PathFinding;
    private IEnemyAttack EnemyAttack;
    private bool isAttacking = false;

    private void Start()
    {
        PathFinding = GetComponent<IPathFinding>();
        EnemyAttack = GetComponent<IEnemyAttack>();
        Player = FindObjectOfType<PlayerController>();

        LevelRules.Instance.AddEnemy();
    }

    private void OnDestroy()
    {
        LevelRules.Instance.KilledEnemy(1);
    }

    private void FixedUpdate()
    {
        bool isMoving = false;
        if (Player != null && !isAttacking)
        {
            //TURN THIS INTO A STATE MACHINE
            if (EnemyAttack.CanAttack(Player.transform.position))
            {
                Movement.RotateTowards(Player.transform.position);
                isAttacking = true;
                Animator.SetTrigger("IsAttacking");
            }
            else
            {
                PathFinding.UpdatePath(Player.transform.position);
                var Dir = PathFinding.GetDirection();

                if (Dir != Vector2.zero)
                {
                    isMoving = true;
                }
                Movement.SetMoveDirection(Dir);
            }
        }
        else
            Movement.SetMoveDirection(Vector2.zero);
        Animator.SetBool("IsMoving", isMoving);
    }

    //Gets called by Animator
    private void Attack()
    {
        EnemyAttack.StartAttack();
        StartCoroutine(WaitForNextAttack());
    }

    private IEnumerator WaitForNextAttack()
    {
        yield return new WaitForSeconds(EnemyAttack.GetAttackDuration());

        isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            AudioManager.Instance.PlaySound("Zombie Death");

            EntityDropTable.GetDrop();

            Destroy(gameObject);
        }
    }


    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.white;
    //    var point = transform.position + transform.up * EnemyAttack.GetAttackRange();

    //    Gizmos.DrawSphere(point, .5f);
    //}
}
