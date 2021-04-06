using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : IDropObject
{
    [SerializeField] private SpriteRenderer WeaponRenderer;
    [SerializeField] private ScriptableDropTable DropTable;

    private WeaponData weapon;

    private void Start()
    {
        AddScriptable(DropTable.GetDrop());
    }

    public override void AddScriptable(IDropScriptableObject dropScriptable)
    {
        if (dropScriptable is null)
        {
            Destroy(gameObject);
        }
        else
        {
            weapon = (WeaponData)dropScriptable;
            WeaponRenderer.sprite = weapon.Icon;
        }
    }

    protected override void AddDropObjectToPlayer(PlayerController playerController)
    {
        playerController.SetWeaponData(weapon);
    }
}
