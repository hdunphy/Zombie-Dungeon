using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private float AttackRange;
    [SerializeField] private float AttackDuration;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private float Damage;
    [SerializeField] private float ProjectileSpeed;
    [SerializeField] private Projectile Projectile;
    [SerializeField] private LayerMask CollisionLayer;

    private float nextAttack;

    public void StartAttack()
    {
        Projectile proj = Instantiate(Projectile, transform.position, transform.rotation);
        proj.Initialize(proj.transform.up * ProjectileSpeed, Damage, gameObject);
        nextAttack = Time.time + AttackSpeed;
    }

    public float GetAttackRange()
    {
        return AttackRange;
    }

    public float GetAttackDuration()
    {
        return AttackSpeed;
    }

    public bool CanAttack(Vector3 playerPostion)
    {
        bool inRange = Vector3.Distance(transform.position, playerPostion) <= AttackRange;
        var hit = Physics2D.Linecast(transform.position, playerPostion, CollisionLayer);
        bool canShoot = Time.time > nextAttack;

        //In Range with no walls in between
        return canShoot && inRange && !hit;
    }
}
