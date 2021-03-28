using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    [SerializeField] private List<WeaponChance> availableWeapons;
    [SerializeField] private SpriteRenderer WeaponRenderer;

    private WeaponData weapon;

    // Start is called before the first frame update
    void Start()
    {
        float roll = UnityEngine.Random.value;
        foreach(WeaponChance _weaponChance in availableWeapons)
        {
            if(roll <= _weaponChance.RollChance)
            {
                weapon = _weaponChance.Weapon;
                break;
            }
        }
        if (weapon == null) weapon = availableWeapons[availableWeapons.Count - 1].Weapon;

        WeaponRenderer.sprite = weapon.Icon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().SetWeaponData(weapon);

            Destroy(gameObject);
        }
    }
}

[Serializable]
public struct WeaponChance
{
    [Range(0, 1)]
    public float RollChance;
    public WeaponData Weapon;
}
