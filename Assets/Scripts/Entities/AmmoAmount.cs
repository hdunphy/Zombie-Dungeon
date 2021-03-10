using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoAmount
{
    public int TotalAmmo { get; private set; }


    public AmmoAmount(int _totalAmmo)
    {
        TotalAmmo = _totalAmmo;
    }

    public void AddAmmo(int ammo)
    {
        TotalAmmo += ammo;
    }

    public int Reload(int ClipSize, int AmmoInClip)
    {
        int _ammoInClip;

        if (TotalAmmo <= 0)
        {
            _ammoInClip = AmmoInClip;
        }
        else if (TotalAmmo + AmmoInClip >= ClipSize)
        {
            TotalAmmo -= (ClipSize - AmmoInClip);
            _ammoInClip = ClipSize;
        }
        else
        {
            _ammoInClip = AmmoInClip + TotalAmmo;
            TotalAmmo = 0;
        }

        return _ammoInClip;
    }
}
