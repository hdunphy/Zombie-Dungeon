using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IDropObject : MonoBehaviour
{
    //void AddDropObjectToPlayer(PlayerController _player);
    //abstract void SuccessfulDrop(Vector3 position);
    public abstract void AddScriptable(IDropScriptableObject dropScriptable);
    protected abstract void AddDropObjectToPlayer(PlayerController playerController);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AddDropObjectToPlayer(collision.GetComponent<PlayerController>());

            Destroy(gameObject);
        }
    }

}
