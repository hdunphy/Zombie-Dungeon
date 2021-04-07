using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IShotType : MonoBehaviour
{
    public abstract IEnumerator Shoot(WeaponData CurrentWeapon);
}
