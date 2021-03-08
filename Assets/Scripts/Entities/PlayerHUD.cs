using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text AmmoAmmountText;

    public static PlayerHUD Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateAmmoAmount(AmmoAmount _ammoAmount)
    {
        UpdateAmmoAmount(_ammoAmount.AmmoInClip, _ammoAmount.TotalAmmo);
    }

    public void UpdateAmmoAmount(int currentClip, int totalAmmo)
    {
        AmmoAmmountText.text = $"{currentClip} / {totalAmmo}";
    }
}
