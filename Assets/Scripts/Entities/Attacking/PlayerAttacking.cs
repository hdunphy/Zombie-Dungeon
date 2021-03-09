using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour, IAttacking
{
    [SerializeField] private WeaponData WeaponData;
    [SerializeField] private LineRenderer FireLine;
    [SerializeField] private SpriteRenderer WeaponRenderer;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private int NewWeaponClipAmount;

    private bool isAttacking;
    private float nextFire;
    //private int ammoInClip;
    //private int totalAmmo;

    private Dictionary<AmmoType, AmmoAmount> heldAmmo;

    private Action attackEvent;

    // Start is called before the first frame update
    void Awake()
    {
        heldAmmo = new Dictionary<AmmoType, AmmoAmount>();

        isAttacking = false;
        nextFire = 0;

        //SetWeaponData(WeaponData);
    }

    // Update is called once per frame
    void Update()
    {
        int ammoInClip = heldAmmo[WeaponData.AmmoType].AmmoInClip;
        if (isAttacking && ammoInClip > 0 && Time.time > nextFire)
        {
            StartCoroutine(Shoot());
            nextFire = Time.time + WeaponData.FireRate;
        }
    }

    private IEnumerator Shoot()
    {
        RaycastHit2D hitPoint = Physics2D.Raycast(FirePoint.position, FirePoint.up, WeaponData.WeaponDistance);

        if (hitPoint)
        {
            if (hitPoint.transform.TryGetComponent(out EnemyController enemy))
            {
                enemy.TakeDamage(WeaponData.Damage);
            }

            FireLine.SetPosition(0, FirePoint.position);
            FireLine.SetPosition(1, hitPoint.point);
        }
        else
        {
            FireLine.SetPosition(0, FirePoint.position);
            FireLine.SetPosition(1, FirePoint.position + FirePoint.up * WeaponData.WeaponDistance);
        }

        TriggerAttackEvent();
        FireLine.enabled = true;

        yield return new WaitForSeconds(WeaponData.ShotDuration);

        FireLine.enabled = false;
        //SHOW IMPACT EFFECT

        heldAmmo[WeaponData.AmmoType].Fire();
        UpdateAmmoHUD();
    }

    private void UpdateAmmoHUD()
    {
        PlayerHUD.Instance.UpdateAmmoAmount(heldAmmo[WeaponData.AmmoType]);
    }

    public void SetWeaponData(WeaponData data)
    {
        WeaponData = data;

        if (!heldAmmo.ContainsKey(data.AmmoType))
        {
            heldAmmo.Add(data.AmmoType, new AmmoAmount(data.ClipSize, data.ClipSize * NewWeaponClipAmount));
            UpdateAmmoHUD();
        }
        else
        {
            AddAmmo(data.ClipSize * NewWeaponClipAmount);
        }
        WeaponRenderer.sprite = data.Sprite;
        FirePoint.localPosition = data.FirePoint;
    }

    public void AddAmmo(int ammo)
    {
        heldAmmo[WeaponData.AmmoType].AddAmmo(ammo);
        UpdateAmmoHUD();
    }

    public void Reload()
    {
        heldAmmo[WeaponData.AmmoType].Reload(WeaponData.ClipSize);
        UpdateAmmoHUD();
    }

    public void ShowWeapon(bool canShow)
    {
        WeaponRenderer.enabled = canShow;
    }

    public void AssignAttackEvent(Action callback)
    {
        attackEvent += callback;
    }

    public void UnAssignAttackEvent(Action callback)
    {
        attackEvent -= callback;
    }

    public void SetIsAttacking(bool _isAttacking)
    {
        isAttacking = _isAttacking;
    }

    public bool GetIsAttacking()
    {
        return isAttacking;
    }

    private void TriggerAttackEvent()
    {
        attackEvent?.Invoke();
    }
}
