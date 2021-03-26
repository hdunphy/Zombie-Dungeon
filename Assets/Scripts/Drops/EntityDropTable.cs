using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityDropTable : MonoBehaviour
{
    [SerializeField] private List<DropChance> drops;

    public void GetDrop()
    {
        float Total = drops.Sum(x => x.RollChance);
        float currentChance = 0;
        float roll = UnityEngine.Random.value;

        foreach(DropChance _drop in drops)
        {
            currentChance += _drop.RollChance;
            if(roll <= currentChance)
            {
                //_drop.DropObject.SuccessfulDrop(transform.position);
                if (!_drop.IsEmpty)
                    Instantiate(_drop.DropObject, transform.position, Quaternion.identity);
                break;
            }
        }
    }
}

[Serializable]
public struct DropChance
{
    [Range(0, 1)]
    public float RollChance;
    public bool IsEmpty;
    public GameObject DropObject;
}
