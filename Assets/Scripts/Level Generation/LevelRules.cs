using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRules : MonoBehaviour
{

    public static LevelRules Instance;

    public float SpawnRate { get; private set; }
    public int SpawnRadius { get; private set; }
    public int EnemyLimit { get; private set; }
    public int NumberOfSpawners { get; private set; }

    private int EnemyCount;
    private int KillCount;

    public Action<bool> EndLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSpawnerData(float _spawnRate, int _spawnRadius, int _enemyLimit, int _numberOfSpawners)
    {
        SpawnRate = _spawnRate;
        SpawnRadius = _spawnRadius;
        EnemyLimit = _enemyLimit;
        NumberOfSpawners = _numberOfSpawners;
    }

    public void KilledEnemy(int enemies)
    {
        EnemyCount -= enemies;
        KillCount += enemies;
        CheckWinCondition();
    }

    public void AddEnemy()
    {
        EnemyCount++;
    }

    public void RemoveSpawner()
    {
        NumberOfSpawners--;
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        //Debug.Log($"Enemy Count {EnemyCount}, Spawner Count {NumberOfSpawners}");
        if(NumberOfSpawners + EnemyCount <= 0)
        {
            EndLevel?.Invoke(true);
        }
    }

    public bool CanSpawnEnemies()
    {
        return EnemyCount < EnemyLimit;
    }
}
