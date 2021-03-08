using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoAmount
{
    public int AmmoInClip { get; private set; }
    public int TotalAmmo { get; private set; }

    public AmmoAmount(int _ammoInClip, int _totalAmmo)
    {
        AmmoInClip = _ammoInClip;
        TotalAmmo = _totalAmmo;
    }

    public void AddAmmo(int ammo)
    {
        TotalAmmo += ammo;
    }

    public void Reload(int ClipSize)
    {
        if (TotalAmmo <= 0)
            return;


        if (TotalAmmo >= ClipSize)
        {
            TotalAmmo -= (ClipSize - AmmoInClip);
            AmmoInClip = ClipSize;
        }
        else
        {
            AmmoInClip = TotalAmmo;
            TotalAmmo = 0;
        }
    }

    public void Fire()
    {
        AmmoInClip--;
    }
}
