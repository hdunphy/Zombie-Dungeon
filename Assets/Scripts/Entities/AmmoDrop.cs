using System;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDrop : IDropObject
{
    //[SerializeField] private List<AmmoChance> AmmoChances;
    [SerializeField] private ScriptableDropTable DropTable;
    [SerializeField] private SpriteRenderer Bullet_L;
    [SerializeField] private SpriteRenderer Bullet_M;
    [SerializeField] private SpriteRenderer Bullet_R;

    private int ammo;
    private AmmoData data;

    private void Start()
    {
        AddScriptable(DropTable.GetDrop());
    }

    public override void AddScriptable(IDropScriptableObject dropScriptable)
    {
        data = (AmmoData)dropScriptable;

        ammo = UnityEngine.Random.Range(data.Min, data.Max);
        Sprite _ammoSprite = data.Sprite;
        Bullet_L.sprite = _ammoSprite;
        Bullet_M.sprite = _ammoSprite;
        Bullet_R.sprite = _ammoSprite;
    }

    protected override void AddDropObjectToPlayer(PlayerController playerController)
    {
        playerController.AddAmmo(ammo, data);
    }
}
