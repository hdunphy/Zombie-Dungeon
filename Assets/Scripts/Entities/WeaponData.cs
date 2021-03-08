using UnityEngine;

public enum AmmoType { Small, Medium, Shotgun }

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/WeaponData")]
public class WeaponData : ScriptableObject
{
    public Sprite Sprite;
    public float Damage;
    public float FireRate;
    public float WeaponDistance;
    public float ShotDuration;
    public int ClipSize;
    public AmmoType AmmoType;
    public Vector3 FirePoint;
}
