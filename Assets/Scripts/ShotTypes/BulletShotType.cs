using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShotType : IShotType
{
    [SerializeField] private LineRenderer FireLine;

    public override IEnumerator Shoot(WeaponData CurrentWeapon)
    {
        RaycastHit2D hitPoint = Physics2D.Raycast(transform.position, transform.up, CurrentWeapon.WeaponDistance);

        //ShotAudio.Play();

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

            FireLine.SetPosition(0, transform.position);
            FireLine.SetPosition(1, hitPoint.point);
        }
        else
        {
            FireLine.SetPosition(0, transform.position);
            FireLine.SetPosition(1, transform.position + transform.up * CurrentWeapon.WeaponDistance);
        }

        //TriggerAttackEvent();
        FireLine.enabled = true;

        yield return new WaitForSeconds(CurrentWeapon.ShotDuration);

        FireLine.enabled = false;
        //SHOW IMPACT EFFECT
    }
}
