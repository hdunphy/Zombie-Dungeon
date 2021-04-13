using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRules : MonoBehaviour
{
    [SerializeField] private float WaveScoreMultiplier;
    [SerializeField] private float EnemyScoreMultiplier;
    [SerializeField] private float SpawnerScoreMultiplier;


    public static LevelRules Instance;

    public float SpawnRate { get; private set; }
    public int SpawnRadius { get; private set; }
    public int EnemyLimit { get; private set; }
    public int NumberOfSpawners { get; private set; }
    public int KillCount { get; private set; }
    public int SpawnerDestroyCount { get; private set; }
    public int WaveNumber { get; private set; }
    public float Score { get; private set; }

    private int EnemyCount;

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

    private void Start()
    {
        WaveNumber = 1;
        PlayerHUD.Instance.SetWaveNumber(WaveNumber);
    }

    public void SetSpawnerData(float _spawnRate, int _spawnRadius, int _enemyLimit)
    {
        SpawnRate = _spawnRate;
        SpawnRadius = _spawnRadius;
        EnemyLimit = _enemyLimit;
    }

    public void KilledEnemy(int enemies)
    {
        EnemyCount -= enemies;
        KillCount += enemies;
        CalculateScore(enemies, EnemyScoreMultiplier);
        CheckWinCondition();
    }

    private void CalculateScore(int count, float multiplier)
    {
        Score += (count *  multiplier) * WaveScoreMultiplier * WaveNumber;
        PlayerHUD.Instance.SetScore(Mathf.RoundToInt(Score));
    }

    public void PlayerDeath()
    {
        EndLevel?.Invoke(false);
    }

    public void AddEnemy()
    {
        EnemyCount++;
    }

    public void AddSpawner()
    {
        NumberOfSpawners++;
    }

    public void RemoveSpawner()
    {
        NumberOfSpawners--;
        SpawnerDestroyCount++;
        CalculateScore(1, SpawnerScoreMultiplier);
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        //Debug.Log($"Enemy Count {EnemyCount}, Spawner Count {NumberOfSpawners}");
        if(NumberOfSpawners + EnemyCount <= 0)
        {
            PlayerHUD.Instance.SetWaveNumber(++WaveNumber);
            EndLevel?.Invoke(true);
        }
    }

    public bool CanSpawnEnemies()
    {
        return EnemyCount < EnemyLimit;
    }
}
