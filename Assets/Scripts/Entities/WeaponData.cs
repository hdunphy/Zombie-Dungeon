using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Drops/WeaponData")]
public class WeaponData : IDropScriptableObject
{
    public Sprite Sprite;
    public Sprite Icon;
    public AudioClip ShotAudioClip;
    
    public new string name;
    public float Damage;
    public float FireRate;
    public float WeaponDistance;
    public float ShotDuration;
    public float ReloadTime;
    public int ClipSize;
    public AmmoData AmmoType;
    public Vector3 FirePoint;
}
