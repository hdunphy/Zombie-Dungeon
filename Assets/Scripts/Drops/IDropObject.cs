using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDropObject
{
    void AddDropObjectToPlayer(PlayerController _player);
    void SuccessfulDrop(Vector3 position);
}
