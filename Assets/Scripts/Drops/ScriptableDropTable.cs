using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScriptableDropTable : MonoBehaviour
{
    [SerializeField] private List<ScriptableDropChance> drops;
    //[SerializeField] private IDropObject DropObject;

    public IDropScriptableObject GetDrop()
    {
        IDropScriptableObject dropScriptable = null;
        float Total = drops.Sum(x => x.RollChance);
        float currentChance = 0;
        float roll = UnityEngine.Random.value;

        foreach (ScriptableDropChance _drop in drops)
        {
            currentChance += _drop.RollChance;
            if (roll <= currentChance)
            {
                //_drop.DropObject.SuccessfulDrop(transform.position);
                if (!_drop.IsEmpty)
                    dropScriptable = _drop.DropScriptable;
                    //DropObject.AddScriptable(_drop.DropScriptable);
                    //_drop.DropScriptable.Drop(DropObject, transform.position);
                break;
            }
        }
        return dropScriptable;
    }
}

[Serializable]
public class ScriptableDropChance
{
    [Range(0, 1)]
    public float RollChance;
    public bool IsEmpty;

    public IDropScriptableObject DropScriptable;
}
