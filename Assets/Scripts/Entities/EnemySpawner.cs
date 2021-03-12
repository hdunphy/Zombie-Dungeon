using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float SpawnRate;
    private int SpawnRadius;
    private float Health;

    public void SetSpawner(float _spawnRate, int _spawnRadius, float _health)
    {
        SpawnRate = _spawnRate;
        SpawnRadius = _spawnRadius;
        Health = _health;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
