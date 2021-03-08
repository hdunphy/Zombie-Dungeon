using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacking
{
    void Reload();
    void AddAmmo(int ammo);
    void SetIsAttacking(bool _isAttacking);
    bool GetIsAttacking();
    void SetWeaponData(WeaponData data);
    void AssignAttackEvent(Action onAttackEvent);
    void UnAssignAttackEvent(Action onAttackEvent);
}
