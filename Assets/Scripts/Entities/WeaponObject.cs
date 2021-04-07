using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponObject
{
    public WeaponData Data { get; private set; }
    public IShotType ShotType { get; private set; }
    public int AmmoInClip { get; set; }

    public WeaponObject(WeaponData _data, int _ammoInClip, IShotType _shotType)
    {
        ShotType = _shotType;
        Data = _data;
        AmmoInClip = _ammoInClip;
    }

    public void Fire()
    {
        AmmoInClip--;
    }

    public IEnumerator Shoot()
    {
        yield return ShotType.Shoot(Data);
    }
}
