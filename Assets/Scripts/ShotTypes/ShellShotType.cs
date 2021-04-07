using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellShotType : IShotType
{
    [SerializeField] private List<LineRenderer> FireLines;
    [SerializeField] private float SpreadAngle;

    public bool debug;

    public override IEnumerator Shoot(WeaponData CurrentWeapon)
    {
        Vector2 position = transform.position;
        Vector3 middle = transform.up;

        for (int i = 0; i < FireLines.Count; i++)
        {
            LineRenderer FireLine = FireLines[i];
            float angle = Random.Range(-SpreadAngle, SpreadAngle);
            Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * middle;

            if (debug)
            {
                Debug.Log($"Line {i}: angle: {angle}\nDir: {direction}, Middle: {middle}");
                Debug.DrawRay(position, direction, Color.white);
            }

            RaycastHit2D hitPoint = Physics2D.Raycast(position, direction, CurrentWeapon.WeaponDistance);

            if (hitPoint)
            {
                if (hitPoint.transform.TryGetComponent(out ITakeDamage damageable) && !hitPoint.transform.TryGetComponent(out PlayerController _))
                {
                    AudioManager.Instance.PlaySound("Bullet Hit");
                    damageable.TakeDamage(CurrentWeapon.Damage / FireLines.Count);
                }
                else
                {
                    AudioManager.Instance.PlaySound("Bullet Miss");
                }

                FireLine.SetPosition(0, position);
                FireLine.SetPosition(1, hitPoint.point);
            }
            else
            {
                FireLine.SetPosition(0, position);
                FireLine.SetPosition(1, position + direction * CurrentWeapon.WeaponDistance);
            }

            FireLine.enabled = true;
            //TODO: IMPACT EFFECT
        }

        yield return new WaitForSeconds(CurrentWeapon.ShotDuration);

        for (int i = 0; i < FireLines.Count; i++)
        {
            FireLines[i].enabled = false;
        }
    }
}
