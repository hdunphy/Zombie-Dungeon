using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour, IAttacking
{
    [SerializeField] private LineRenderer FireLine;
    [SerializeField] private SpriteRenderer WeaponRenderer;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private int NewWeaponClipAmount;
    [SerializeField] private AudioSource ShotAudio;

    private bool isAttacking;
    private float nextFire;
    private int currentWeaponIndex;
    private WeaponData CurrentWeapon => weapons[currentWeaponIndex].Data;
    private int CurrentAmmo => weapons[currentWeaponIndex].AmmoInClip;

    private List<WeaponObject> weapons;
    private Dictionary<AmmoType, AmmoAmount> heldAmmo;

    private Action attackEvent;
    private Coroutine ReloadCoroutine;

    // Start is called before the first frame update
    void Awake()
    {
        heldAmmo = new Dictionary<AmmoType, AmmoAmount>();
        weapons = new List<WeaponObject>();

        isAttacking = false;
        nextFire = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (weapons.Count <= 0)
            return;

        if (isAttacking && CurrentAmmo > 0 && Time.time > nextFire)
        {
            StartCoroutine(Shoot());
            nextFire = Time.time + CurrentWeapon.FireRate;
        }
    }

    private IEnumerator Shoot()
    {
        RaycastHit2D hitPoint = Physics2D.Raycast(FirePoint.position, FirePoint.up, CurrentWeapon.WeaponDistance);

        ShotAudio.Play();

        if (hitPoint)
        {
            if (hitPoint.transform.TryGetComponent(out ITakeDamage damageable) && !hitPoint.transform.TryGetComponent(out PlayerController _))
            {
                AudioManager.Instance.PlaySound("Bullet Hit");
                damageable.TakeDamage(CurrentWeapon.Damage);
            }
            else
            {
                AudioManager.Instance.PlaySound("Bullet Miss");
            }

            FireLine.SetPosition(0, FirePoint.position);
            FireLine.SetPosition(1, hitPoint.point);
        }
        else
        {
            FireLine.SetPosition(0, FirePoint.position);
            FireLine.SetPosition(1, FirePoint.position + FirePoint.up * CurrentWeapon.WeaponDistance);
        }

        TriggerAttackEvent();
        FireLine.enabled = true;

        yield return new WaitForSeconds(CurrentWeapon.ShotDuration);

        FireLine.enabled = false;
        //SHOW IMPACT EFFECT


        weapons[currentWeaponIndex].Fire();
        UpdateAmmoHUD();
    }

    private void UpdateAmmoHUD()
    {
        PlayerHUD.Instance.UpdateAmmoAmount(CurrentAmmo, heldAmmo[CurrentWeapon.AmmoType].TotalAmmo);
    }

    public void SetWeaponData(WeaponData data)
    {
        if (ReloadCoroutine != null)
            StopCoroutine(ReloadCoroutine);

        int index = weapons.FindIndex(x => x.Data == data);
        if (index < 0)
        {
            weapons.Add(new WeaponObject(data, data.ClipSize));
            currentWeaponIndex = weapons.Count - 1;
            AudioManager.Instance.PlaySound("PickupWeapon");
        }

        AddAmmo(data.ClipSize * NewWeaponClipAmount, data.AmmoType);

        UpdateWeapon();
        nextFire = Time.time;// + switching time ?
    }

    private void UpdateWeapon()
    {
        ShotAudio.clip = CurrentWeapon.ShotAudioClip;

        WeaponRenderer.sprite = CurrentWeapon.Sprite;
        FirePoint.localPosition = CurrentWeapon.FirePoint;
        PlayerHUD.Instance.UpdateAmmoType(CurrentWeapon.AmmoType);
        PlayerHUD.Instance.SetSelectedGun(CurrentWeapon.Icon);
    }

    public void AddAmmo(int ammo, AmmoType ammoType)
    {
        if (!heldAmmo.ContainsKey(ammoType))
        {
            heldAmmo.Add(ammoType, new AmmoAmount(0));
        }

        heldAmmo[ammoType].AddAmmo(ammo);
        AudioManager.Instance.PlaySound("PickupAmmo");
        UpdateAmmoHUD();
    }

    public void Reload()
    {
        if (CurrentWeapon.ClipSize == CurrentAmmo)
        {
            AudioManager.Instance.PlaySound("Reload");
        }
        else
        {
            if (ReloadCoroutine != null)
                StopCoroutine(ReloadCoroutine);
            ReloadCoroutine = StartCoroutine(ReloadWeapon(CurrentWeapon.ReloadTime));
        }
    }

    private IEnumerator ReloadWeapon(float seconds)
    {
        PlayerHUD.Instance.Reload(seconds);
        nextFire = Time.time + seconds;

        if (seconds > 0)
        {
            AudioManager.Instance.PlaySound("Clip out");

            yield return new WaitForSeconds(seconds / 2f);

            if (seconds > 1)
            {
                AudioManager.Instance.PlaySound("Clip In");
            }

            yield return new WaitForSeconds(seconds / 2f);
        }

        weapons[currentWeaponIndex].AmmoInClip = heldAmmo[CurrentWeapon.AmmoType].Reload(CurrentWeapon.ClipSize, CurrentAmmo);
        AudioManager.Instance.PlaySound("Reload");
        UpdateAmmoHUD();
    }

    public void ShowWeapon(bool canShow)
    {
        WeaponRenderer.enabled = canShow;
    }

    public void CycleWeapon(int direction)
    {
        currentWeaponIndex = ++currentWeaponIndex >= weapons.Count ? 0 : currentWeaponIndex;

        AudioManager.Instance.PlaySound("CycleWeapon");
        UpdateWeapon();
        UpdateAmmoHUD();
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
