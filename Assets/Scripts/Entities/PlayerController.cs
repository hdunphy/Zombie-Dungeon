using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(IAttacking))]
public class PlayerController : MonoBehaviour, ITakeDamage
{
    [SerializeField] private Animator Animator;
    [SerializeField] private Movement Movement;

    [SerializeField] private float Health;
    [SerializeField] private WeaponData StartingWeapon;

    private IAttacking Attacking;
    private Vector2 inputVector;
    private float CurrentHealth;

    private const float FireAnimationTime = .183f;

    private void Start()
    {
        Attacking = GetComponent<IAttacking>();
        Attacking.AssignAttackEvent(OnAttackEvent);
        SetWeaponData(StartingWeapon);

        CurrentHealth = Health;
    }

    private void OnDestroy()
    {
        Attacking.UnAssignAttackEvent(OnAttackEvent);
    }

    private void Update()
    {
        bool isMoving = false;
        if (!Attacking.GetIsAttacking())
        {
            isMoving = inputVector != Vector2.zero;
            Movement.SetCanMove(true);
            //Rb.velocity = (inputVector * MoveSpeed * Time.deltaTime);
        }
        else
        {
            Movement.SetCanMove(false);
            //Rb.velocity = Vector2.zero;
        }

        Animator.SetBool("IsMoving", isMoving);
        Attacking.ShowWeapon(!isMoving);
    }

    public void OnMove(CallbackContext callback)
    {
        inputVector = callback.ReadValue<Vector2>();
        Movement.SetMoveDirection(inputVector);
    }

    public void OnFire(CallbackContext callback)
    {
        Attacking.SetIsAttacking(callback.performed);
    }

    public void OnReload(CallbackContext callback)
    {
        if (callback.started)
        {
            Attacking.Reload();
        }
    }

    public void OnWeaponCycleNext(CallbackContext callback)
    {
        if (callback.started)
        {
            Attacking.CycleWeapon(1);
        }
    }

    public void OnWeaponCyclePrev(CallbackContext callback)
    {
        if (callback.started)
        {
            Attacking.CycleWeapon(-1);
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        PlayerHUD.Instance.SetHealthPercent(CurrentHealth / Health);

        if (CurrentHealth <= 0)
        {
            LevelRules.Instance.PlayerDeath();
            AudioManager.Instance.PlaySound("Player Died");
            Destroy(gameObject);
        }
    }

    public void AddHealth(float healthAmount)
    {
        AudioManager.Instance.PlaySound("Health Pickup");
        CurrentHealth = Mathf.Clamp(CurrentHealth + healthAmount, 0, Health);
        PlayerHUD.Instance.SetHealthPercent(CurrentHealth / Health);
    }

    public void AddAmmo(int ammo, AmmoData ammoType)
    {
        Attacking.AddAmmo(ammo, ammoType);
    }

    public void SetWeaponData(WeaponData data)
    {
        Attacking.SetWeaponData(data);
        Animator.SetFloat("FireRate", data.FireRate / FireAnimationTime);
    }

    private void OnAttackEvent()
    {
        Animator.SetTrigger("Shoot");
    }
}
