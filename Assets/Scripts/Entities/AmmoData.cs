using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoData", menuName = "Drops/AmmoData")]
public class AmmoData : IDropScriptableObject
{
    public new string name;
    public Sprite Sprite;
    public Sprite Icon;
    public int Min;
    public int Max;
}
