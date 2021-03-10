using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject
{
    public WeaponData Data { get; private set; }
    public int AmmoInClip { get; set; }

    public WeaponObject(WeaponData _data, int _ammoInClip)
    {
        Data = _data;
        AmmoInClip = _ammoInClip;
    }

    public void Fire()
    {
        AmmoInClip--;
    }
}
