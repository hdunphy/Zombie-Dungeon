using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttack
{
    void StartAttack();
    float GetAttackRange();
    float GetAttackDuration();
    bool CanAttack(Vector3 playerPostion);
}
